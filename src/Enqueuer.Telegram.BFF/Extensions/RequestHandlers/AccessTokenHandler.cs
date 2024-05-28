using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Queueing.Contract.V1.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Enqueuer.Telegram.BFF.Extensions.RequestHandlers;

public class AccessTokenHandler : DelegatingHandler
{
    private readonly IIdentityClient _identityClient;
    private readonly QueueingClientOptions _clientOptions;
    private AccessToken? _accessToken;

    public AccessTokenHandler(IIdentityClient identityClient, IOptions<QueueingClientOptions> clientOptions)
    {
        _identityClient = identityClient;
        _clientOptions = clientOptions.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization == null || _accessToken == null)
        {
            if (_accessToken != null && !_accessToken.HasExpired)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(_accessToken.Type, _accessToken.Value);
                return await base.SendAsync(request, cancellationToken);
            }

            _accessToken = await _identityClient.GetAccessTokenAsync(_clientOptions.RequiredScopes, cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue(_accessToken.Type, _accessToken.Value);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
