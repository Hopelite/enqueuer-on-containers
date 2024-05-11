namespace Enqueuer.Identity.Authorization.Exceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException()
    {
    }

    public UserDoesNotExistException(string? message)
        : base(message)
    {
    }

    public UserDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
