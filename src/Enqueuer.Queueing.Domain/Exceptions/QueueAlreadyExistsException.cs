namespace Enqueuer.Queueing.Domain.Exceptions;

public class QueueAlreadyExistsException : DomainException
{
    public QueueAlreadyExistsException()
    {
    }

    public QueueAlreadyExistsException(string? message)
        : base(message)
    {
    }

    public QueueAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
