namespace Enqueuer.Telegram.Notifications.Localization;

public class MessageDoesNotExistException : Exception
{
    public MessageDoesNotExistException()
    {
    }

    public MessageDoesNotExistException(string? message)
        : base(message)
    {
    }

    public MessageDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
