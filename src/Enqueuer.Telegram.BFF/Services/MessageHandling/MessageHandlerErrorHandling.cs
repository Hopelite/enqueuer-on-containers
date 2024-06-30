using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Handlers;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Services.MessageHandling;

/// <summary>
/// Synchronizes user info and notifies about internal errors.
/// </summary>
internal class MessageHandlerErrorHandling(
    IMessageHandler messageHandlerToDecorate,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    ILogger<MessageHandlerErrorHandling> logger) : IMessageHandler
{
    private readonly IMessageHandler _decoratedMessageHandler = messageHandlerToDecorate;
    private readonly ITelegramBotClient _telegramClient = telegramClient;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly ILogger<MessageHandlerErrorHandling> _logger = logger;

    public async Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        try
        {
            await _decoratedMessageHandler.HandleAsync(messageContext, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during the message '{MessageId}' handling sent from the chat '{ChatId}'.", messageContext.MessageId, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }

    private Task<Message> NotifyUserAboutInternalErrorAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var errorMessage = _localizationProvider.GetMessage(MessageKeys.GeneralErrorInternal, new MessageParameters(messageContext.Chat.Culture));
        return _telegramClient.SendTextMessageAsync(
            chatId: messageContext.Chat.Id,
            text: errorMessage,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
