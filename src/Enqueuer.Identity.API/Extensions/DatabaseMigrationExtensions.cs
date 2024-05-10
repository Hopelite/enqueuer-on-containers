using Enqueuer.Identity.API.Resources;
using Enqueuer.Identity.Authorization.Extensions;
using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Identity.API.Extensions;

public static class DatabaseMigrationExtensions
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            dbContext.Database.Migrate();

            CreatePredefinedScopes(dbContext);
            CreatePredefinedRoles(dbContext);
        }

        return app;
    }

    private static void CreatePredefinedScopes(IdentityContext dbContext)
    {
        var predefinedScopes = ResourceHelper.GetResource<IEnumerable<Scope>>("predefined_scopes.json");

        var existingScopes = dbContext.Scopes.ToDictionary(s => s.Name);
        foreach (var scope in predefinedScopes)
        {
            if (existingScopes.ContainsKey(scope.Name))
            {
                // TODO: check the existing scope's parent and child scopes
            }
            else
            {
                // TODO: update the child scopes
                var newScope = scope.MapRecursive<Scope, Persistence.Models.Scope>((scope, nestedScopes) => new Persistence.Models.Scope
                {
                    Name = scope.Name,
                    Children = nestedScopes
                },
                scope => scope.ChildScopes);

                dbContext.Scopes.Add(newScope);
            }
        }

        dbContext.SaveChanges();
    }

    private static void CreatePredefinedRoles(IdentityContext dbContext)
    {
        var predefinedRoles = ResourceHelper.GetResource<IEnumerable<Role>>("predefined_roles.json");

        var existingRoles = dbContext.Roles.Include(r => r.Scopes)
            .ToDictionary(r => r.Name);
        var availableScopes = dbContext.Scopes.ToDictionary(s => s.Name);

        foreach (var role in predefinedRoles)
        {
            if (existingRoles.TryGetValue(role.Name, out var existingRole))
            {
                existingRole.Scopes = MapRoleScopes(role, availableScopes);
                dbContext.Roles.Update(existingRole);
            }
            else
            {
                var newRole = new Persistence.Models.Role
                {
                    Name = role.Name,
                    Scopes = MapRoleScopes(role, availableScopes)
                };

                dbContext.Roles.Add(newRole);
            }
        }

        dbContext.SaveChanges();
    }

    private static IEnumerable<Persistence.Models.Scope> MapRoleScopes(Role role, Dictionary<string, Persistence.Models.Scope> availableScopes)
    {
        return role.Scopes?.Select(s => availableScopes.TryGetValue(s.Name, out var scope)
                        ? scope
                        : throw new InvalidOperationException($"Cannot assign non-existing scope '{s.Name}'.")).ToList()
                        ?? new List<Persistence.Models.Scope>();
    }
}
