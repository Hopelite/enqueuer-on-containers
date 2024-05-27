using Enqueuer.Telegram.BFF.Core.Models.Common;
using Enqueuer.Telegram.Shared.Types;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;
using User = Enqueuer.Telegram.BFF.Core.Models.Common.User;

namespace Enqueuer.Telegram.BFF.Core.Models.Callbacks;

/// <summary>
/// Contains all neccessary callback data used in the application.
/// </summary>
public class CallbackContext
{
    private CallbackContext()
    {
    }

    /// <summary>
    /// The unique identifier for this callback's query.
    /// </summary>
    public required string QueryId { get; init; }

    /// <summary>
    /// The unique identifier of the message within the boundaries of the chat the callback is attached to.
    /// </summary>
    public required int MessageId { get; init; }

    /// <summary>
    /// The user who sent this callback.
    /// </summary>
    public required User Sender { get; init; }

    /// <summary>
    /// The group from which this callback was sent.
    /// </summary>
    public required Group Chat { get; init; }

    /// <summary>
    /// Deserialized callback data.
    /// </summary>
    public required CallbackData CallbackData { get; init; }

    /// <summary>
    /// Tries to create a <paramref name="callbackContext"/> from <paramref name="callbackQuery"/>.
    /// </summary>
    /// <returns>True, if <paramref name="callbackContext"/> created successfully; otherwise false.</returns>
    public static bool TryCreate(CallbackQuery callbackQuery, [NotNullWhen(returnValue: true)] out CallbackContext? callbackContext)
    {
        callbackContext = null;
        if (callbackQuery.From == null || callbackQuery.Message == null || callbackQuery.Data == null)
        {
            return false;
        }

        CallbackData? callbackData = JsonConvert.DeserializeObject<CallbackData?>(callbackQuery.Data);
        if (callbackData == null)
        {
            return false;
        }

        callbackContext = new CallbackContext()
        {
            QueryId = callbackQuery.Id,
            MessageId = callbackQuery.Message.MessageId,
            CallbackData = callbackData,
            Sender = new User
            {
                Id = callbackQuery.From.Id,
                FirstName = callbackQuery.From.FirstName,
                LastName = callbackQuery.From.LastName,
                LanguageCode = callbackQuery.From.LanguageCode ?? DefaultValues.DefaultLanguageCode,
            },
            Chat = new Group
            {
                Id = callbackQuery.Message.Chat.Id,
                Title = callbackQuery.Message.Chat.Title,
                Type = (ChatType)callbackQuery.Message.Chat.Type,
            }
        };

        return true;
    }
}