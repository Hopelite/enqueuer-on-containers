using Enqueuer.Identity.OAuth.Models.Enums;
using Microsoft.AspNetCore.Http.Extensions;

namespace Enqueuer.Identity.OAuth.Models;

/// <summary>
/// The response to successful authorization request containing the authorization code.
/// </summary>
public class AuthorizationResponse
{
    private readonly Uri _redirectUri;

    internal AuthorizationResponse(in AuthorizationCode code, string? state, Uri redirectUri)
    {
        Code = code;
        State = state;
        _redirectUri = redirectUri;
    }

    /// <summary>
    /// The issued authorization code.
    /// </summary>
    public AuthorizationCode Code { get; }

    /// <summary>
    /// Not null if the "state" parameter was present in the client authorization request.
    /// The exact value received from the client.
    /// </summary>
    public string? State { get; }

    /// <summary>
    /// Gets the complete redirect_uri with the "state" and "code" parameters included.
    /// </summary>
    public Uri RedirectUri
    {
        get
        {
            var queryParameters = new Dictionary<string, string>()
            {
                { QueryParameter.AuthorizationResponse.AuthorizationCode, Code.Value }
            };

            if (State != null)
            {
                queryParameters[QueryParameter.AuthorizationResponse.State] = State;
            }

            return new UriBuilder(_redirectUri)
            {
                Query = new QueryBuilder(queryParameters).ToQueryString().Value
            }.Uri;
        }
    }
}
