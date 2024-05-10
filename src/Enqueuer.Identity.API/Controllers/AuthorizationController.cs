using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.Authorization.Extensions;
using Enqueuer.Identity.Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = Enqueuer.Identity.Authorization.IAuthorizationService;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [AllowedScope("user:create", "user:update")]
    [HttpPut("users/{user_id}")]
    public async Task<IActionResult> CreateOrUpdateUserAsync(CreateOrUpdateUserRequest request, CancellationToken cancellationToken)
    {
        await _authorizationService.CreateOrUpdateUserAsync(new User(request.UserId, request.FirstName, request.LastName), cancellationToken);
        return Created();
    }

    [HttpGet]
    public Task<IActionResult> CheckAccessAsync([FromQuery] CheckAccessQueryParameters query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{resource_id}")]
    public async Task<IActionResult> GrantAccessAsync(GrantAccessQueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            await _authorizationService.GrantAccessAsync(query.RecourceId, query.UserId, new Role(query.Role), cancellationToken);
        }
        catch (Exception)
        {

            throw;
        }

        return Ok();
    }

    [HttpDelete]
    public Task<IActionResult> RevokeAccessAsync([FromQuery] RevokeAccessQueryParameters query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static Role MapToRole(CreateOrUpdateRoleRequest request)
    {
        var scopes = new List<Authorization.Models.Scope>();
        foreach (var scope in request.Scopes)
        {
            scopes.Add(scope.MapRecursive<Parameters.Scope, Authorization.Models.Scope>(
                (scope, nestedScopes) => new Authorization.Models.Scope(scope.Name, nestedScopes),
                scope => scope.ChildScopes));
        }

        return new Role(request.RoleName, scopes);
    }
}
