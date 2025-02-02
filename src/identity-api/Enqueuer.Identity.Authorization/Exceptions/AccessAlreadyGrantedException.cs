namespace Enqueuer.Identity.Authorization.Exceptions;

public class AccessAlreadyGrantedException : Exception
{
    public AccessAlreadyGrantedException()
    {
    }

    public AccessAlreadyGrantedException(string? message)
        : base(message)
    {
    }

    public AccessAlreadyGrantedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
