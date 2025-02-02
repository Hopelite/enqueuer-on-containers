using System;

namespace Enqueuer.Identity.Contract.V1.OAuth.Exceptions
{
    public class OAuthClientException : Exception
    {
        public OAuthClientException()
        {
        }

        public OAuthClientException(string message)
            : base(message)
        {
        }

        public OAuthClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
