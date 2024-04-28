namespace Enqueuer.Identity.API.Services.Grants;

public interface IAuthorizationGrant
{
    /// <summary>
    /// The type of the authorization grant.
    /// </summary>
    string Type { get; }
}
