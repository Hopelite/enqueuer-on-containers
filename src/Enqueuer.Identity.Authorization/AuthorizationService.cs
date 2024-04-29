using Enqueuer.Identity.Authorization.Exceptions;
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
