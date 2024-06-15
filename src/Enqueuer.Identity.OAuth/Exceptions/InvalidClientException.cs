namespace Enqueuer.Identity.OAuth.Exceptions;

public class InvalidClientException : AuthorizationException
{
    private const string InvalidClientErrorCode = "invalid_client";

    public InvalidClientException()
        : base(InvalidClientErrorCode)
    {
    }

    public InvalidClientException(string? message)
        : base(InvalidClientErrorCode, message)
    {
    }

    public InvalidClientException(string? message, Exception? innerException)
        : base(InvalidClientErrorCode, message, innerException)
    {
    }
}