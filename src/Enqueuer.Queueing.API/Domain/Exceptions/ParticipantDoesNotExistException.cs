namespace Enqueuer.Queueing.API.Domain.Exceptions;

public class ParticipantDoesNotExistException : Exception
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
