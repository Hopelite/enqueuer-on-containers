namespace Enqueuer.Identity.Authorization.Scopes;

public class NestingIsTooDeepException : Exception
{
    public NestingIsTooDeepException()
    {
    }

    public NestingIsTooDeepException(string? message)
        : base(message)
    {
    }

    public NestingIsTooDeepException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
