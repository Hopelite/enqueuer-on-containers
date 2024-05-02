using Enqueuer.Identity.Authorization.Exceptions;
using Enqueuer.Identity.Authorization.Extensions;
using Enqueuer.Identity.Persistence;
using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }

    public async Task<bool> CheckAccessAsync(Uri resourceUri, long userId, Models.Scope scope, CancellationToken cancellationToken)
    {
        // TODO: consider to use query/stored procedure here
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

        var userToCheckAccess = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        if (userToCheckAccess == null)
        {
            throw new UserDoesNotExistException($"User '{userId}' does not exist in database.");
        }

        var resourceToGrantAccess = await dbContext.Resources.FirstOrDefaultAsync(r => r.Uri == resourceUri, cancellationToken);
        if (resourceToGrantAccess == null)
        {
            throw new ResourceDoesNotExistException($"Resource '{resourceUri}' does not exist in database.");
        }

        var grantedAccess = await dbContext.UserResourceRoles
            .Where(r => r.UserId == userToCheckAccess.Id && r.ResourceId == resourceToGrantAccess.Id)
            .Include(r => r.Role)
            .ThenInclude(r => r.Scopes)
            .ThenInclude(s => s.Children) // Temporary workaround - there could be more than one row of child scopes
            .FirstOrDefaultAsync(cancellationToken);

        if (grantedAccess == null)
        {
            return false;
        }

        // TODO: possibly cache role scopes list to reduce JOINs
        var scopes = grantedAccess.Role.Scopes.SelectMany(s => s.Children)
            .Concat(grantedAccess.Role.Scopes)
            .Distinct();

        return scopes.Any(s => s.Name.Equals(scope.Name));
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
            resourceToGrantAccess = new Resource
            {
                Uri = resourceUri,
            };
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
