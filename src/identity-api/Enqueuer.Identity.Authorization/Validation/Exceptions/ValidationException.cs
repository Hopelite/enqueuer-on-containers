namespace Enqueuer.Identity.Authorization.Validation.Exceptions;

/// <summary>
/// Base class for all validation exceptions.
/// </summary>
public class ValidationException : Exception
{
    protected ValidationException()
    {
    }

    protected ValidationException(string? message)
        : base(message)
    {
    }

    /// <remarks>Intended to be used to envelop system exceptions which occured during the validation procces.</remarks>
    public ValidationException(string? message, Exception innerException)
        : base(message, innerException ?? throw new ArgumentNullException(nameof(innerException)))
    {
    }
}
