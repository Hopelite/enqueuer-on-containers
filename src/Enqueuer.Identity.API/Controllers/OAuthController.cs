using Enqueuer.EventBus.Abstractions;
using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.OAuth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Scope = Enqueuer.Identity.Authorization.Models.Scope;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[Route("oauth2")]
public class OAuthController(IOAuthService authorizationService, IEventBusClient busClient, ILogger<OAuthController> logger) : ControllerBase
{
    private readonly IOAuthService _authorizationService = authorizationService;
    private readonly IEventBusClient _busClient = busClient;
    private readonly ILogger<OAuthController> _logger = logger;

    [HttpGet("authorize")]
    public async Task<IActionResult> AuthorizeAsync([FromQuery] AuthorizeQueryParameters query, CancellationToken cancellationToken)
    {
        var request = new OAuth.Models.AuthorizationRequest(query.ResponseType, query.ClientId, query.RedirectUri, new OAuth.Models.Scope(query.Scope), query.State);

        try
        {
            var response = await _authorizationService.AuthorizeAsync(request, cancellationToken);
            return Redirect(response.RedirectUri.AbsoluteUri);
        }
        catch (AuthorizationException ex)
        {
            _logger.LogInformation(ex, "Authorization request for the client '{ClientId}' has failed.", query.ClientId);

            // TODO: if redirect URI is null - use the registered one
            return Redirect(request.RedirectUri.AbsoluteUri);
        }
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetAccessTokenAsync([FromQuery] GetAccessTokenQueryParameters query, CancellationToken cancellationToken)
    {
        // TODO: add token to cache
        OAuth.Models.AccessTokenResponse token;
        try
        {
            var scopes = query.Scopes.Select(scope => new Scope(scope, childScopes: null)).ToArray();
            token = await _authorizationService.GetAccessTokenAsync(new OAuth.Models.AccessTokenRequest(query.Grant), cancellationToken);
        }
        catch (UnsupportedGrantTypeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AuthorizationException ex)
        {
            return Unauthorized(ex.Message);
        }

        // TODO: possibly create separate GetAccessTokenResponse
        var response = new GetAccessTokenResponse(token.Value, token.Type, token.ExpiresIn);
        return Ok(response);
    }
}