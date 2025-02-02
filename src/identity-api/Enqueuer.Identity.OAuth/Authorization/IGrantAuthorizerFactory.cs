using Enqueuer.OAuth.Core.Exceptions;

namespace Enqueuer.Identity.OAuth.Authorization;

public interface IGrantAuthorizerFactory
{
    /// <summary>
    /// Gets the <see cref="IGrantAuthorizer"/> for the <paramref name="grantType"/>.
    /// </summary>
    /// <exception cref="UnsupportedGrantTypeException">Thrown, if the <paramref name="grantType"/> is unsupported.</exception>
    IGrantAuthorizer GetAuthorizerFor(string grantType);
}
