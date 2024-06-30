using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Tokens;
using Enqueuer.OAuth.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Enqueuer.Identity.OAuth;

public class OAuthService : IOAuthService
{
    private readonly IAccessTokenManager _tokenManager;
    private readonly ILogger<OAuthService> _logger;

    public OAuthService(IAccessTokenManager tokenManager, ILogger<OAuthService> logger)
    {
        _tokenManager = tokenManager;
        _logger = logger;
    }

    public Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (AuthorizationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // All internal exceptions must be consealed from the client
            _logger.LogError(ex, "An internal error occured during authorization request.");
            throw new ServerErrorException(ex);
        }
    }

    public async Task<AccessTokenResponse> GetAccessTokenAsync(AccessTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = await _tokenManager.RequestAccessTokenAsync(request.AuthorizationGrant, cancellationToken);
            return new AccessTokenResponse(accessToken.Value, accessToken.Type, accessToken.ExpiresIn);
        }
        catch (AuthorizationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // All internal exceptions must be consealed from the client
            _logger.LogError(ex, "An internal error occured during access token request.");
            throw new ServerErrorException(ex);
        }
    }
}
