namespace Enqueuer.Identity.OAuth.Exceptions;

public class InvalidGrantException : AuthorizationException
{
    private const string InvalidGrantErrorCode = "invalid_grant";

    public InvalidGrantException()
        : base(InvalidGrantErrorCode)
    {
    }

    public InvalidGrantException(string? message)
        : base(InvalidGrantErrorCode, message)
    {
    }

    public InvalidGrantException(string? message, Exception? innerException)
        : base(InvalidGrantErrorCode, message, innerException)
    {
    }
}