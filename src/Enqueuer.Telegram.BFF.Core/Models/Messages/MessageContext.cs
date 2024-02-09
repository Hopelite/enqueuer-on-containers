using Enqueuer.Telegram.BFF.Core.Models.Common;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;
using User = Enqueuer.Telegram.BFF.Core.Models.Common.User;

namespace Enqueuer.Telegram.BFF.Core.Models.Messages;

/// <summary>
/// Contains all neccessary message data used in the application.
/// </summary>
public class MessageContext
{
    private MessageContext()
    {
    }

    /// <summary>
    /// The <see cref="MessageType"/> of the Telegram message.
    /// </summary>
    public MessageType Type => Command == null
     ? MessageType.PlainText
     : MessageType.Command;

    /// <summary>
    /// The unique identifier of the message within the boundaries of the chat.
    /// </summary>
    public required int MessageId { get; init; }

    /// <summary>
    /// Optional. The command specified in the message. 
    /// </summary>
    public CommandContext? Command { get; init; }

    /// <summary>
    /// The user who sent this message.
    /// </summary>
    public required User Sender { get; init; }

    /// <summary>
    /// The group to which this message was sent.
    /// </summary>
    public required Group Chat { get; init; }

    /// <summary>
    /// Tries to create a <paramref name="messageContext"/> from <paramref name="message"/>.
    /// </summary>
    /// <returns>True, if <paramref name="messageContext"/> created successfully; otherwise false.</returns>
    public static bool TryCreate(Message message, [NotNullWhen(returnValue: true)] out MessageContext? messageContext)
    {
        messageContext = null;
        if (message?.Text == null || message.From == null)
        {
            return false;
        }

        if (message.Text.TryGetCommand(out var command))
        {
            // TODO: disable for plain text messaging is completed (ex. Queue name for Callback button)
            return false;
        }

        messageContext = new MessageContext
        {
            MessageId = message.MessageId,
            Command = command,
            Sender = new User
            {
                Id = message.From.Id,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
            },
            Chat = new Group
            {
                Id = message.Chat.Id,
                Title = message.Chat.Title!,
                Type = (ChatType)message.Chat.Type,
            }
        };

        return true;
    }
}
