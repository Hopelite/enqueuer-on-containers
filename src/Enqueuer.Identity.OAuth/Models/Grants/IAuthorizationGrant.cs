namespace Enqueuer.Identity.OAuth.Models.Grants;

/// <summary>
/// Defines a credential representing the resource owner's authorization to the client to obtain an access token.
/// </summary>
public interface IAuthorizationGrant
{
    /// <summary>
    /// The type of the authorization grant.
    /// </summary>
    string Type { get; }
}