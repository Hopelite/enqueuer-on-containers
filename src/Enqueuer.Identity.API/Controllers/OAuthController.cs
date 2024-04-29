using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Contract.V1;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("api/oauth2")]
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
        // TODO: verify client credentials

        // TODO: add token to cache

        var token = _authorizationService.GetAccessTokenAsync(query.Grant, query.Scopes);

        var response = new GetAccessTokenResponse(token.Token, token.Type, token.ExpiresIn);
        return Ok(response);
    }
}