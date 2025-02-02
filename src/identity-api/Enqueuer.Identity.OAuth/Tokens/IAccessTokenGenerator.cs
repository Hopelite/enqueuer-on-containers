using Enqueuer.OAuth.Core.Tokens;

namespace Enqueuer.Identity.OAuth.Tokens;

public interface IAccessTokenGenerator
{
    /// <summary>
    /// Generates <see cref="AccessToken"/> using the provided <paramref name="context"/>.
    /// </summary>
    AccessToken GenerateToken(AccessTokenContext context);
}
