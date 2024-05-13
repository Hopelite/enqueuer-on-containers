using System;

namespace Enqueuer.Identity.Contract.V1.Exceptions
{
    public class InvalidCredentialsException : IdentityClientException
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
