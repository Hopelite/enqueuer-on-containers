using System;

namespace Enqueuer.Identity.Contract.V1.OAuth.Exceptions
{
    public class InvalidCredentialsException : OAuthClientException
    {
        public InvalidCredentialsException()
        {
        }

        public InvalidCredentialsException(string message)
            : base(message)
        {
        }

        public InvalidCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
