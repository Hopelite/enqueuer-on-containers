using Enqueuer.Identity.OAuth.Extensions;

namespace Enqueuer.Identity.OAuth.Models;

/// <summary>
/// Represents the issued authorization code.
/// </summary>
public readonly struct AuthorizationCode
{
    internal AuthorizationCode(string value)
    {
        Value = value.ThrowIfNullOrWhitespace(nameof(value));
    }

    /// <summary>
    /// The value of the authorization code.
    /// </summary>
    public string Value { get; }
}
