namespace Enqueuer.Identity.Authorization.Exceptions;

public class RoleDoesNotExistException : Exception
{
    public RoleDoesNotExistException()
    {
    }

    public RoleDoesNotExistException(string? message)
        : base(message)
    {
    }

    public RoleDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
