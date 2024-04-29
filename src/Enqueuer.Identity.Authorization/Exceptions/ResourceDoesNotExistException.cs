namespace Enqueuer.Identity.Authorization.Exceptions;

public class ResourceDoesNotExistException : Exception
{
    public ResourceDoesNotExistException()
    {
    }

    public ResourceDoesNotExistException(string? message)
        : base(message)
    {
    }

    public ResourceDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
