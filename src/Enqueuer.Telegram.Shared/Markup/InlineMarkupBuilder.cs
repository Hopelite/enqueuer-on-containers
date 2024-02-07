using Enqueuer.Telegram.Shared.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Shared.Markup;

public class InlineMarkupBuilder(IDataSerializer dataSerializer) : IInlineMarkupBuilder
{
    private readonly Queue<IEnumerable<InlineKeyboardButton>> _markup = new();
    private readonly Queue<InlineKeyboardButton> _currentRow = new();
    private readonly IDataSerializer _dataSerializer = dataSerializer;

    public IMarkupBuilder<InlineKeyboardMarkup, InlineKeyboardButton> OnNewRow
    {
        get
        {
            if (_currentRow.Count == 0)
            {
                throw new InvalidOperationException("Cannot move to the next buttons row, while current is empty: inline keyboard doesn't support empty ones.");
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
        // TODO: Consider throwing an error since it's invalid behavior to have empty inline markup

        if (_currentRow.Count != 0)
        {
            _markup.Enqueue(_currentRow);
        }

        return new InlineKeyboardMarkup(_markup);
    }
}
