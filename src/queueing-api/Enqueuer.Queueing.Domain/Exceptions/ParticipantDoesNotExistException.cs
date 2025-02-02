namespace Enqueuer.Queueing.Domain.Exceptions;

public class ParticipantDoesNotExistException : DomainException
{
    public ParticipantDoesNotExistException()
    {
    }

    public ParticipantDoesNotExistException(string? message)
        : base(message)
    {
    }

    public ParticipantDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
