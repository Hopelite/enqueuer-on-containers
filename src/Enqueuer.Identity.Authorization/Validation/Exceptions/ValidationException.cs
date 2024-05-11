namespace Enqueuer.Identity.Authorization.Validation.Exceptions;

/// <summary>
/// Base class for all validation exceptions.
/// </summary>
public abstract class ValidationException : Exception
{
    protected ValidationException()
    {
    }

    protected ValidationException(string? message)
        : base(message)
    {
    }

    protected ValidationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
