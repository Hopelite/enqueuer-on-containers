using Enqueuer.Telegram.Shared.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Shared.Markup;

/// <summary>
/// Provides methods to builds the <typeparamref name="TMarkup"/> from provided <typeparamref name="TButton"/>s.
/// </summary>
public interface IMarkupBuilder<TMarkup, TButton>
    where TMarkup: IReplyMarkup
    where TButton: IKeyboardButton
{
    /// <summary>
    /// Specifies that the following buttons will be added to a new row. 
    /// </summary>
    IMarkupBuilder<TMarkup, TButton> OnNewRow { get; }

    /// <summary>
    /// Adds <typeparamref name="TButton"/> created using <paramref name="buttonFactory"/> to <typeparamref name="TMarkup"/>.
    /// </summary>
    IMarkupBuilder<TMarkup, TButton> Add(CreateMarkupButtonDelegate<TButton> buttonFactory);

    /// <summary>
    /// Builds the current <typeparamref name="TMarkup"/> from provided <typeparamref name="TButton"/>s.
    /// </summary>
    TMarkup Build();
}

public delegate TButton CreateMarkupButtonDelegate<TButton>(IDataSerializer serializer) where TButton: IKeyboardButton;