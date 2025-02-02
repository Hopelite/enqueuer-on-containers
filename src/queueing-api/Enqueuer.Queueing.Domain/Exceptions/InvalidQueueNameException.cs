namespace Enqueuer.Queueing.Domain.Exceptions;

public class InvalidQueueNameException : DomainException
{
    public InvalidQueueNameException()
    {
    }

    public InvalidQueueNameException(string? message)
        : base(message)
    {
    }

    public InvalidQueueNameException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
