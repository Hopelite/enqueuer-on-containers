using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

/// <summary>
/// Contains query parameters for the request to receive authorization grant.
/// </summary>
public class AuthorizeQueryParameters
{
    /// <summary>
    /// The value indicating what client expects to receive if authorization is successful.
    /// </summary>
    [BindProperty(Name = "response_type")]
    public string ResponseType { get; set; } = null!;

    /// <summary>
    /// The client identifier used to authenticate the client to the authorization server.
    /// </summary>
    [BindProperty(Name = "client_id")]
    public string ClientId { get; set; } = null!;

    [BindProperty(Name = "redirect_uri")]
    public Uri? RedirectUri { get; set; }

    /// <summary>
    /// The scope of the access request.
    /// </summary>
    [BindProperty(Name = "scope")]
    public ScopeCollection Scope { get; set; } = null!;

    /// <summary>
    /// An opaque value used by the client to maintain state between the request and callback.
    /// </summary>
    [BindProperty(Name = "state")]
    public string? State { get; set; }
}
