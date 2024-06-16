using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.OAuth.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("oauth2")]
public class OAuthController(IOAuthService authorizationService) : ControllerBase
{
    private readonly IOAuthService _authorizationService = authorizationService;

    [HttpGet("authorize")]
    public Task<IActionResult> Authorize([FromQuery] AuthorizeQueryParameters query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetAccessToken([FromQuery] GetAccessTokenQueryParameters query, CancellationToken cancellationToken)
    {
        AccessTokenResponse token;
        try
        {
            var request = new AccessTokenRequest(query.Grant);
            token = await _authorizationService.GetAccessTokenAsync(request, cancellationToken);
        }
        catch (UnsupportedGrantTypeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }

        var response = new GetAccessTokenResponse(token.Value, token.Type, token.ExpiresIn);
        return Ok(response);
    }
}