namespace Enqueuer.Queueing.Domain.Exceptions;

public class PositionReservedException : DomainException
{
    public PositionReservedException()
    {
    }

    public PositionReservedException(string? message)
        : base(message)
    {
    }

    public PositionReservedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
