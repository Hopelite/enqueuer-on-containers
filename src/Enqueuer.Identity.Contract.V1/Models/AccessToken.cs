using System;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class AccessToken
    {
        private readonly DateTime _expires;

        public AccessToken(string value, string type, TimeSpan expiresIn)
        {
            Value = value;
            Type = type;
            ExpiresIn = expiresIn;
            _expires = DateTime.UtcNow.Add(ExpiresIn);
        }

        public string Value { get; }

        public string Type { get; }

        public TimeSpan ExpiresIn { get; }

        public bool HasExpired => _expires <= DateTime.UtcNow;
    }
}
