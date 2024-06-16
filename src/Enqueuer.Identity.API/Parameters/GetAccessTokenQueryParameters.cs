using Enqueuer.Identity.API.Parameters.Binders;
using Enqueuer.OAuth.Core.Tokens.Grants;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

/// <summary>
/// Contains query parameters for the request to get an access token.
/// </summary>
public class GetAccessTokenQueryParameters
{
    /// <summary>
    /// The credential representing the resource owner's authorization used by the client to obtain an access token.
    /// </summary>
    [ModelBinder(BinderType = typeof(AuthorizationGrantModelBinder))]
    public IAuthorizationGrant Grant { get; set; } = null!;
}
