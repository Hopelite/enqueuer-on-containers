namespace Enqueuer.Queueing.Domain.Exceptions;

public class ParticipantAlreadyExistsException : DomainException
{
    public ParticipantAlreadyExistsException()
    {
    }

    public ParticipantAlreadyExistsException(string? message)
        : base(message)
    {
    }

    public ParticipantAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
