using Enqueuer.Telegram.Shared.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Shared.Markup;

public class InlineMarkupBuilder : IInlineMarkupBuilder
{
    private readonly Queue<IEnumerable<InlineKeyboardButton>> _markup;
    private readonly Queue<InlineKeyboardButton> _currentRow;
    private readonly IDataSerializer _dataSerializer;

    public InlineMarkupBuilder(IDataSerializer dataSerializer)
    {
        _markup = new Queue<IEnumerable<InlineKeyboardButton>>();
        _currentRow = new Queue<InlineKeyboardButton>();
        _dataSerializer = dataSerializer;
    }

    public IMarkupBuilder<InlineKeyboardMarkup, InlineKeyboardButton> OnNewRow
    {
        get
        {
            if (_currentRow.Count == 0)
            {
                // TODO: Consider throwing an error since it's invalid behavior to have empty inline markup
                return this;
            }

            _markup.Enqueue(_currentRow);
            _currentRow.Clear();
            return this;
        }
    }

    public IMarkupBuilder<InlineKeyboardMarkup, InlineKeyboardButton> Add(CreateMarkupButtonDelegate<InlineKeyboardButton> buttonFactory)
    {
        var button = buttonFactory(_dataSerializer);
        _currentRow.Enqueue(button);
        return this;
    }

    public InlineKeyboardMarkup Build()
    {
        if (_currentRow.Count != 0)
        {
            _markup.Enqueue(_currentRow);
        }

        // TODO: Consider throwing an error since it's invalid behavior to have empty inline markup

        return new InlineKeyboardMarkup(_markup);
    }
}
