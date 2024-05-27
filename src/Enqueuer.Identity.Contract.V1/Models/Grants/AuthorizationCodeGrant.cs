using System;

namespace Enqueuer.Identity.Contract.V1.Models.Grants
{
    public class AuthorizationCodeGrant
    {
        public AuthorizationCodeGrant(string code, Uri? redirectUri, string? clientId)
        {
            Code = code;
            RedirectUri = redirectUri;
            ClientId = clientId;
        }

        public string Code { get; }

        public Uri? RedirectUri { get; }

        public string? ClientId { get; }
    }
}
