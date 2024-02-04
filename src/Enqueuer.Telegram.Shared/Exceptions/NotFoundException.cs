using Telegram.Bot.Exceptions;

namespace Enqueuer.Telegram.Shared.Exceptions;

public class NotFoundException : ApiRequestException
{
    public NotFoundException(string message)
        : this(message, (int)Exceptions.ErrorCode.NotFound)
    {
    }

    public NotFoundException(string message, int errorCode)
        : base(message, errorCode)
    {
    }

    public NotFoundException(string message, int errorCode, Exception innerException)
        : base(message, errorCode, innerException)
    {
    }
}
