namespace Enqueuer.Telegram.BFF.Core.Models.Common;

public enum ChatType
{
    /// <summary>
    /// Normal one to one <see cref="Chat"/>
    /// </summary>
    Private = 1,

    /// <summary>
    /// Normal group chat
    /// </summary>
    Group,

    /// <summary>
    /// A channel
    /// </summary>
    Channel,

    /// <summary>
    /// A supergroup
    /// </summary>
    Supergroup,

    /// <summary>
    /// “sender” for a private chat with the inline query sender
    /// </summary>
    Sender
}
