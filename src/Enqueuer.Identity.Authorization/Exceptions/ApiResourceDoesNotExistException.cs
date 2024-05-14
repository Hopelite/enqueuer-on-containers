namespace Enqueuer.Identity.Authorization.Exceptions;

public abstract class ApiResourceDoesNotExistException : Exception
{
    protected ApiResourceDoesNotExistException()
    {
    }

    protected ApiResourceDoesNotExistException(string? message)
        : base(message)
    {
    }

    protected ApiResourceDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
