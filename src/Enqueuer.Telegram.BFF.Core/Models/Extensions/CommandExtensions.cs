using Enqueuer.Telegram.BFF.Core.Models.Common;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Telegram.BFF.Core.Models.Extensions;

public static class CommandExtensions
{
    private const char Whitespace = ' ';
    private const char CommandDeclaration = '/';
    private const char BotNameSeparator = '@';

    /// <summary>
    /// Gets queue name and possibly position from <paramref name="commandContext"/>.
    /// </summary>
    public static QueueNameContext GetQueueName(this CommandContext commandContext)
    {
        if (commandContext.Parameters.Length == 0)
        {
            return new QueueNameContext(QueueName: string.Empty, Position: null);
        }

        if (commandContext.Parameters.Length > 1 && int.TryParse(commandContext.Parameters[^1], out var position))
        {
            return new QueueNameContext(QueueName: string.Join(separator: Whitespace, commandContext.Parameters[..^1]), position);
        }

        return new QueueNameContext(QueueName: string.Join(separator: Whitespace, commandContext.Parameters), Position: null);
    }

    /// <summary>
    /// Gets queue name from <paramref name="query"/>.
    /// </summary>
    /// <param name="startIndex">Start index where queue name starts.</param>
    public static string GetQueueName(this string[] query, int startIndex = 1) // TODO: add unit tests
    {
        if (query.Length == 2 && int.TryParse(query[^1], out var _))
        {
            return query[^1];
        }

        if (int.TryParse(query[^1], out var _))
        {
            return string.Join(separator: Whitespace, query[startIndex..^1]);
        }

        return string.Join(separator: Whitespace, query[startIndex..]);
    }

    /// <summary>
    /// Splits <paramref name="messageText"/> to words by removing whitespaces.
    /// </summary>
    /// <returns>Array of message words.</returns>
    public static string[] SplitToWords(this string messageText)
    {
        return messageText.Split(separator: Whitespace, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Tries to create <paramref name="commandContext"/> from <paramref name="messageText"/>.
    /// </summary>
    public static bool TryGetCommand(this string messageText, [NotNullWhen(returnValue: true)] out CommandContext? commandContext)
    {
        commandContext = null;
        if (messageText.TryGetCommand(out var command, out var parameters))
        {
            commandContext = new CommandContext(command, parameters);
            return true;
        }

        return false;
    }

    private static bool TryGetCommand(this string messageText, [NotNullWhen(returnValue: true)] out string? command, [NotNullWhen(returnValue: true)] out string[]? parameters)
    {
        command = null;
        parameters = null;

        var commandWords = messageText.SplitToWords();
        if (commandWords.Length == 0)
        {
            return false;
        }

        command = commandWords[0];
        if (command[0] != CommandDeclaration)
        {
            return false;
        }

        var botNamePosition = messageText.IndexOf(BotNameSeparator);
        if (botNamePosition > 0)
        {
            command = command[..botNamePosition];
        }

        parameters = commandWords[1..];
        return true;
    }
}

public record QueueNameContext(string QueueName, int? Position);