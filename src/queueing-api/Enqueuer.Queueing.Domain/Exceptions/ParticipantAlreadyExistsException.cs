namespace Enqueuer.Queueing.Domain.Exceptions;

public class ParticipantAlreadyExistsException : DomainException
{
    public ParticipantAlreadyExistsException(string queueName, long participantId)
        : this(queueName, participantId, null)
    {
    }

    public ParticipantAlreadyExistsException(string queueName, long participantId, string? message)
        : this(queueName, participantId, message, null)
    {
    }

    public ParticipantAlreadyExistsException(string queueName, long participantId, string? message, Exception? innerException)
        : base(message, innerException)
    {
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public string QueueName { get; }

    public long ParticipantId { get; }
}
