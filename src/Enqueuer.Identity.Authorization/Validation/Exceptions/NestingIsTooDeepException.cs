namespace Enqueuer.Identity.Authorization.Validation.Exceptions;

public class NestingIsTooDeepException : ValidationException
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
