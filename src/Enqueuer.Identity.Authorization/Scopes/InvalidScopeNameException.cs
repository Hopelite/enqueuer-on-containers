namespace Enqueuer.Identity.Authorization.Scopes;

public class InvalidScopeNameException : Exception
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
