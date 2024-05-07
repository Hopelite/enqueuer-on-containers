using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.Authorization.Exceptions;
using Enqueuer.Identity.Authorization.Grants.Validation;
using Enqueuer.Identity.Authorization.OAuth;
using Enqueuer.Identity.Contract.V1;
using Microsoft.AspNetCore.Mvc;
using Scope = Enqueuer.Identity.Authorization.Models.Scope;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("oauth2")]
public class OAuthController : ControllerBase
{
    private readonly IOAuthService _authorizationService;

    public OAuthController(IOAuthService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpGet("authorize")]
    public Task<IActionResult> Authorize([FromQuery] AuthorizeQueryParameters query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetAccessToken([FromQuery] GetAccessTokenQueryParameters query, CancellationToken cancellationToken)
    {
        // TODO: add token to cache
        AccessToken token;
        try
        {
            var scopes = query.Scopes.Select(scope => new Scope(scope, childScopes: null)).ToArray();
            token = await _authorizationService.GetAccessTokenAsync(query.Grant, scopes, cancellationToken);
        }
        catch (UsupportedGrantTypeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }

        var response = new GetAccessTokenResponse(token.Token, token.Type, token.ExpiresIn);
        return Ok(response);
    }
}