using Enqueuer.Identity.Authorization.Exceptions;
using Enqueuer.Identity.Authorization.Extensions;
using Enqueuer.Identity.Persistence;
using Enqueuer.Identity.Persistence.Models;
using Enqueuer.Identity.Persistence.Procedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Enqueuer.Identity.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthorizationService(IServiceScopeFactory scopeFactory)
    {
        _serviceScopeFactory = scopeFactory;
    }

    public async Task CreateOrUpdateRoleAsync(Models.Role role, CancellationToken cancellationToken)
    {
        var roleToSet = new Role
        {
            Name = role.Name,
            Scopes = role.Scopes.Select(s => s.MapRecursive<Models.Scope, Scope>((scope, nestedScopes) => new Scope
            {
                Name = scope.Name,
                Children = nestedScopes
            },
            scope => scope.ChildScopes)).ToArray()
        };

        using var serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();
        using (var transaction = dbContext.Database.BeginTransaction()) // TODO: consider isolation levels
        {
            // TODO: remove this workaround
            var existingScopes = await dbContext.Scopes.AsNoTrackingWithIdentityResolution().ToDictionaryAsync(s => s.Name, cancellationToken);

            var scopesToAssign = new List<Scope>();
            foreach (var scope in roleToSet.Scopes)
            {
                if (existingScopes.TryGetValue(scope.Name, out var existingScope))
                {
                    scopesToAssign.Add(existingScope);
                }
                else
                {
                    scopesToAssign.Add(scope);
                }
            }

            var existingRole = dbContext.Roles
                .Include(r => r.Scopes)
                .FirstOrDefault(r => r.Name == role.Name);

            if (existingRole != null)
            {
                existingRole.Scopes = roleToSet.Scopes;
                dbContext.Update(existingRole);
            }
            else
            {
                dbContext.Roles.Add(roleToSet);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContext.Database.CommitTransactionAsync(cancellationToken);
        }
    }

    public async Task CreateOrUpdateUserAsync(Models.User user, CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId, cancellationToken);
        if (existingUser != null)
        {
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            dbContext.Users.Update(existingUser);
        }
        else
        {
            var newUser = new User
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            dbContext.Users.Add(newUser);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CheckAccessAsync(Uri resourceUri, long userId, Models.Scope scope, CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

        var connection = (NpgsqlConnection)dbContext.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken);

        var command = CheckUserAccessProcedure.GetCommand(resourceUri, userId, scope.Name);
        command.Connection = connection;

        var hasAccess = await command.ExecuteScalarAsync(cancellationToken);
        return (bool)hasAccess!;
    }

    public async Task GrantAccessAsync(Uri resourceUri, long granteeId, Models.Role role, CancellationToken cancellationToken)
    {
        // TODO: consider to use query/stored procedure here
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

        var roleToAssign = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name.Equals(role.Name), cancellationToken);
        if (roleToAssign == null)
        {
            throw new RoleDoesNotExistException($"Role '{role.Name}' does not exist in database.");
        }

        var userToGrantAccess = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == granteeId, cancellationToken);
        if (userToGrantAccess == null)
        {
            throw new UserDoesNotExistException($"User '{granteeId}' does not exist in database. Unable to assign role to unknown user.");
        }

        var resourceToGrantAccess = await dbContext.Resources.FirstOrDefaultAsync(r => r.Uri == resourceUri, cancellationToken);
        if (resourceToGrantAccess == null)
        {
            // TODO: add all parent resources (like groups/ for groups/12 and groups/12/queues/ for groups/12/queues/Test
            resourceToGrantAccess = new Resource
            {
                Uri = resourceUri,
            };
        }

        var existingAccess = await dbContext.UserResourceRoles.FindAsync(new object[] { userToGrantAccess.Id, resourceToGrantAccess.Id }, cancellationToken);
        if (existingAccess != null)
        {
            throw new AccessAlreadyGrantedException(
                $"User with ID '{granteeId}' already has access to the resource '{resourceUri}'. In order to assign different role, you must revoke existing access first.");
        }

        var newAccess = new UserResourceRoles
        {
            User = userToGrantAccess,
            Resource = resourceToGrantAccess,
            Role = roleToAssign
        };

        dbContext.UserResourceRoles.Add(newAccess);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeAccessAsync(Uri resourceUri, long granteeId, CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

        var userToRevokeAccess = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == granteeId, cancellationToken);
        if (userToRevokeAccess == null)
        {
            throw new UserDoesNotExistException($"User '{granteeId}' does not exist in database.");
        }

        var resourceToRevokeAccess = await dbContext.Resources.FirstOrDefaultAsync(r => r.Uri == resourceUri, cancellationToken);
        if (resourceToRevokeAccess == null)
        {
            throw new ResourceDoesNotExistException($"Resource '{resourceUri}' does not exist in database.");
        }

        var accessToRevoke = await dbContext.UserResourceRoles.FindAsync(new object[] { userToRevokeAccess.Id, resourceToRevokeAccess.Id }, cancellationToken);
        if (accessToRevoke == null)
        {
            // TODO: consider to throw exception here
            return;
        }

        dbContext.UserResourceRoles.Remove(accessToRevoke);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
