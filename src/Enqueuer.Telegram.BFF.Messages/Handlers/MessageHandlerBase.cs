using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public abstract class MessageHandlerBase(
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider) : IMessageHandler
{
    protected readonly ITelegramBotClient telegramClient = telegramClient;
    protected readonly ILocalizationProvider localizationProvider = localizationProvider;

    public abstract Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken);

    protected Task NotifyUserAboutInternalErrorAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var errorMessage = localizationProvider.GetMessage(MessageKeys.GeneralErrorInternal, new MessageParameters(messageContext.Chat.Culture));
        return telegramClient.SendTextMessageAsync(
            chatId: messageContext.Chat.Id,
            text: errorMessage,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
