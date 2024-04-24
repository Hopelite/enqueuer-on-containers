namespace Enqueuer.Queueing.Domain.Exceptions;

public class QueueDoesNotExistException : DomainException
{
    public QueueDoesNotExistException(string queueName)
        : this(queueName, null)
    {
    }

    public QueueDoesNotExistException(string queueName, string? message)
        : this(queueName, message, null)
    {
    }

    public QueueDoesNotExistException(string queueName, string? message, Exception? innerException)
        : base(message, innerException)
    {
        QueueName = queueName;
    }

    public string QueueName { get; }
}
