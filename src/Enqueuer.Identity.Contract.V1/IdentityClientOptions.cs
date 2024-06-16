using System;

namespace Enqueuer.Identity.Contract.V1
{
    public class IdentityClientOptions
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        private string _clientSecret = null!;
        private string _clientId = null!;
        private Uri _baseAddress = null!;
        private string[] _requiredScopes = null!;
        public const int DefaultRetries = 3;

        public Uri BaseAddress
        {
            get => _baseAddress;
            set => _baseAddress = value ?? throw new ArgumentNullException(nameof(BaseAddress));
        }

        public string ClientId
        { 
            get => _clientId;
            set => _clientId = ValidateRequiredOption(value, nameof(ClientId));
        }

        public string ClientSecret
        {
            get => _clientSecret;
            set => _clientSecret = ValidateRequiredOption(value, nameof(ClientSecret));
        }

        public TimeSpan Timeout { get; set; } = DefaultTimeout;

        public int MaxRetries { get; set; } = DefaultRetries;

        /// <summary>
        /// Whether to cache the access tokens. Set to true by default.
        /// </summary>
        public bool CacheToken { get; set; } = true;

        public string[] RequiredScopes
        {
            get => _requiredScopes;
            set => _requiredScopes = value ?? throw new ArgumentNullException(nameof(_requiredScopes));
        }

        private static string ValidateRequiredOption(string value, string optionName)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException(optionName)
                : value;
        }
    }
}
