namespace Enqueuer.Identity.OAuth.Exceptions;

/// <summary>
/// The base exception thrown in case of an authorization error.
/// </summary>
public class AuthorizationException : Exception
{
    public AuthorizationException(string errorCode)
        : this(errorCode, message: null)
    {
    }

    public AuthorizationException(string errorCode, string? message)
        : this(errorCode, message, innerException: null)
    {
    }

    public AuthorizationException(string errorCode, string? message, Exception? innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// A single ASCII error code from the list of error responses.
    /// </summary>
    public string ErrorCode { get; }

    // TODO: possibly add state here
}
