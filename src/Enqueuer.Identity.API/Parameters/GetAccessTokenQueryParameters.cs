using Enqueuer.Identity.Authorization.Grants;
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

    /// <summary>
    /// The scope of the access request.
    /// </summary>
    [BindProperty(Name = "scope")]
    public ScopeCollection Scopes { get; set; } = null!;
}
