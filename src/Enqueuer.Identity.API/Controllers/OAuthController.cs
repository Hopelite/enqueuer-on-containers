using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.OAuth.Core.Enums;
using Enqueuer.OAuth.Core.Exceptions;
using Enqueuer.OAuth.Core.Helpers;
using Enqueuer.OAuth.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("oauth2")]
public class OAuthController : ControllerBase
{
    private readonly IOAuthService _authorizationService;
    private readonly ILogger<OAuthController> _logger;

    public OAuthController(IOAuthService authorizationService, ILogger<OAuthController> logger)
    {
        _authorizationService = authorizationService;
        _logger = logger;
    }

    [HttpGet("authorize")]
    public async Task<IActionResult> Authorize([FromQuery] AuthorizeQueryParameters query, CancellationToken cancellationToken)
    {
        var scope = new Scope(query.Scope);
        var request = new AuthorizationRequest(query.ResponseType, query.ClientId, query.RedirectUri, scope, query.State);

        try
        {
            var response = await _authorizationService.AuthorizeAsync(request, cancellationToken);
            return Redirect(response.RedirectUri.AbsoluteUri);
        }
        catch (ServerErrorException ex)
        {
            _logger.LogError(ex, "An internal server error occured during the authorization request handling for the client '{ClientId}'.", query.ClientId);

            // TODO: if redirect URI is null - use the registered one
            return RedirectToRedirectUri(request.RedirectUri, ex.GetQueryParameters(), query.State);
        }
        catch (AuthorizationException ex)
        {
            _logger.LogInformation(ex, "Authorization request for the client '{ClientId}' has failed.", query.ClientId);

            // TODO: if redirect URI is null - use the registered one
            return RedirectToRedirectUri(request.RedirectUri, ex.GetQueryParameters(), query.State);
        }
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
            return BadRequest(ex.GetQueryParameters());
        }
        catch (ServerErrorException ex)
        {
            _logger.LogError(ex, "An internal server error occured during the access token reqest.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.GetQueryParameters());
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.GetQueryParameters());
        }

        var response = new GetAccessTokenResponse(token.Value, token.Type, token.ExpiresIn);
        return Ok(response);
    }

    private RedirectResult RedirectToRedirectUri(Uri redirectUri, IDictionary<string, string> queryParameters, string? state)
    {
        if (!string.IsNullOrWhiteSpace(state))
        {
            queryParameters[QueryParameter.AuthorizationResponse.State] = state;
        }

        var completeUri = redirectUri.AppendQuery(queryParameters);
        return Redirect(completeUri.AbsoluteUri);
    }
}