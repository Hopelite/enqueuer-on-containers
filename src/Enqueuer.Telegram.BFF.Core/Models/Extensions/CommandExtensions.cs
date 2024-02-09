﻿using Enqueuer.Telegram.BFF.Core.Models.Common;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Telegram.BFF.Core.Models.Extensions;

public static class CommandExtensions
{
    private const char Whitespace = ' ';
    private const char CommandDeclaration = '/';
    private const char BotNameSeparator = '@';

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
