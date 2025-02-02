namespace Enqueuer.Queueing.Domain.Exceptions;

public class PositionReservedException : DomainException
{
    public PositionReservedException(string queueName, uint position)
        : this(queueName, position, null)
    {
    }

    public PositionReservedException(string queueName, uint position, string? message)
        : this(queueName, position, message, null)
    {
    }

    public PositionReservedException(string queueName, uint position, string? message, Exception? innerException)
        : base(message, innerException)
    {
        QueueName = queueName;
        Position = position;
    }

    public string QueueName { get; }

    public uint Position { get; }
}
