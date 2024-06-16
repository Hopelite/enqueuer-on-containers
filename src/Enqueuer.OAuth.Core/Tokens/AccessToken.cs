using System;

namespace Enqueuer.OAuth.Core.Tokens
{
    public class AccessToken
    {
        private readonly DateTime _expires;

        public AccessToken(string value, string type, TimeSpan expiresIn)
        {
            ThrowIfNullOrWhiteSpace(value, nameof(value));
            ThrowIfNullOrWhiteSpace(type, nameof(type));

            Value = value;
            Type = type;
            ExpiresIn = ValidateLifetime(expiresIn);
            _expires = DateTime.UtcNow.Add(ExpiresIn);
        }

        /// <summary>
        /// The value of the access token.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The type of the access token.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The lifetime of the issued access token.
        /// </summary>
        public TimeSpan ExpiresIn { get; }

        public bool HasExpired => _expires <= DateTime.UtcNow;

        private static TimeSpan ValidateLifetime(in TimeSpan expiresIn)
        {
            if (expiresIn.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expiresIn), "Access token lifetime can't be negative.");
            }

            return expiresIn;
        }

        private static void ThrowIfNullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
