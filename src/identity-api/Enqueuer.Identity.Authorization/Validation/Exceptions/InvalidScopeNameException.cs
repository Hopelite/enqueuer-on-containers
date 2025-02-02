namespace Enqueuer.Identity.Authorization.Validation.Exceptions;

public class InvalidScopeNameException : ValidationException
{
    public InvalidScopeNameException()
    {
    }

    public InvalidScopeNameException(string? message)
        : base(message)
    {
    }

    public InvalidScopeNameException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
