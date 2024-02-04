using Telegram.Bot.Exceptions;

namespace Enqueuer.Telegram.Shared.Exceptions;

public class TelegramExceptionsParser : IExceptionParser
{
    public ApiRequestException Parse(ApiResponse apiResponse)
    {
        if (apiResponse.ErrorCode == (int)ErrorCode.NotFound)
        {
            return new NotFoundException(apiResponse.Description);
        }

        return new(apiResponse.Description, apiResponse.ErrorCode, apiResponse.Parameters);
    }
}
