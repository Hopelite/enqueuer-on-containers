namespace Enqueuer.Queueing.Domain.Exceptions;

public class QueueDoesNotExistException : Exception
{
    public QueueDoesNotExistException()
    {
    }

    public QueueDoesNotExistException(string? message)
        : base(message)
    {
    }

    public QueueDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
