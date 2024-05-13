using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.Authorization.Exceptions;
using Enqueuer.Identity.Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = Enqueuer.Identity.Authorization.IAuthorizationService;
using Scope = Enqueuer.Identity.Authorization.Models.Scope;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(IAuthorizationService authorizationService) : ControllerBase
{
    private readonly IAuthorizationService _authorizationService = authorizationService;

    [AllowedScope("user:create", "user:update", "user")]
    [HttpPut($"users/{{{CreateOrUpdateUserRequest.UserIdRouteParameter}}}")]
    public async Task<IActionResult> CreateOrUpdateUserAsync(CreateOrUpdateUserRequest request, CancellationToken cancellationToken)
    {
        await _authorizationService.CreateOrUpdateUserAsync(new User(request.UserId, request.FirstName, request.LastName), cancellationToken);
        return Created(); // TODO: change to Ok in case of update
    }

    [HttpGet("{*resource_id}")]
    public async Task<IActionResult> CheckAccessAsync(CheckAccessRequest request, CancellationToken cancellationToken)
    {
        bool hasAccess;
        try
        {
            hasAccess = await _authorizationService.CheckAccessAsync(request.RecourceId, request.UserId, new Scope(request.Scope), cancellationToken);
        }
        catch (UserDoesNotExistException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ResourceDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }

        return hasAccess ? Ok() : NotFound();
    }

    [AllowedScope("access:grant", "access")]
    [HttpPut("{*resource_id}")]
    public async Task<IActionResult> GrantAccessAsync(GrantAccessRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _authorizationService.GrantAccessAsync(request.RecourceId, request.UserId, new Role(request.Role), cancellationToken);
        }
        catch (RoleDoesNotExistException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserDoesNotExistException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AccessAlreadyGrantedException ex)
        {
            return Conflict(ex.Message);
        }

        return Created();
    }

    [AllowedScope("access:revoke", "access")]
    [HttpDelete("{*resource_id}")]
    public async Task<IActionResult> RevokeAccessAsync(RevokeAccessRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _authorizationService.RevokeAccessAsync(request.RecourceId, request.UserId, cancellationToken);
        }
        catch (Exception)
        {

            throw;
        }

        return Ok();
    }
}
