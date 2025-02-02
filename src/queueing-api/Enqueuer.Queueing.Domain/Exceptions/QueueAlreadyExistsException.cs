namespace Enqueuer.Queueing.Domain.Exceptions;

public class QueueAlreadyExistsException : DomainException
{
    public QueueAlreadyExistsException(string queueName)
        : this(queueName, null)
    {
    }

    public QueueAlreadyExistsException(string queueName, string? message)
        : this(queueName, message, null)
    {
    }

    public QueueAlreadyExistsException(string queueName, string? message, Exception? innerException)
        : base(message, innerException)
    {
        QueueName = queueName;
    }

    public string QueueName { get; }
}
