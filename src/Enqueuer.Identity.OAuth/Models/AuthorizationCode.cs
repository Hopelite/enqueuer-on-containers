namespace Enqueuer.Identity.OAuth.Models;

/// <summary>
/// Represents the issued authorization code.
/// </summary>
public readonly struct AuthorizationCode
{
    internal AuthorizationCode(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        Value = value;
    }

    /// <summary>
    /// The value of the authorization code.
    /// </summary>
    public string Value { get; }
}