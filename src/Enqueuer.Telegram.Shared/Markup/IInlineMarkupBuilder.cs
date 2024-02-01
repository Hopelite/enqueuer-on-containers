using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Shared.Markup;

/// <summary>
/// Defines the implementation of the <see cref="IMarkupBuilder{TMarkup, TButton}"/> for <see cref="InlineKeyboardMarkup"/>.
/// </summary>
public interface IInlineMarkupBuilder : IMarkupBuilder<InlineKeyboardMarkup, InlineKeyboardButton>
{
}
