namespace Enqueuer.Queueing.API.Domain.Exceptions;

public class ParticipantAlreadyExistsException : Exception
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
