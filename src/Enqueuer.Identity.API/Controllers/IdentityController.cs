using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Contract.V1;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public IdentityController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetAccessToken([FromQuery] GetAccessTokenQueryParameters query, CancellationToken cancellationToken)
    {
        // TODO: verify client credentials

        // TODO: add token to cache

        var token = _authorizationService.GetAccessToken(query.Grant, query.Scopes);

        var response = new GetAccessTokenResponse(token.Token, token.Type, token.ExpiresIn);
        return Ok(response);
    }
}