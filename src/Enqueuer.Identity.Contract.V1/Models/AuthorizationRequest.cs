using System;
using System.Collections.Generic;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class AuthorizationRequest
    {
        public AuthorizationRequest(string responseType, string clientId, Uri? redirectUri, string? scope, string? state)
        {
            ResponseType = responseType;
            ClientId = clientId; // TODO: validate
            RedirectUri = redirectUri;
            Scope = scope;
            State = state;
        }

        public string ResponseType { get; }

        public string ClientId { get; }

        public Uri? RedirectUri { get; }

        public string? Scope { get; }

        public string? State { get; set; }

        internal IDictionary<string, string> GetQueryParameters()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "response_type", ResponseType },
                {     "client_id", ClientId },
            };

            AddIfNotNull(parameters, "redirect_uri", RedirectUri?.ToString());
            AddIfNotNull(parameters, "scope", Scope);
            AddIfNotNull(parameters, "state", State);

            return parameters;
        }

        // TODO: move to generic extension method
        private static void AddIfNotNull(Dictionary<string, string> parameters, string parameter, string? value)
        {
            if (value != null)
            {
                parameters[parameter] = value;
            }
        }
    }
}
