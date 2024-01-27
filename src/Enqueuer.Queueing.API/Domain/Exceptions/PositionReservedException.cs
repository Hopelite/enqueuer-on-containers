namespace Enqueuer.Queueing.API.Domain.Exceptions;

public class PositionReservedException : Exception
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
