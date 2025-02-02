using System;

namespace Enqueuer.Identity.Contract.V1.Exceptions
{
    public class IdentityClientException : Exception
    {
        public IdentityClientException()
        {
        }

        public IdentityClientException(string message)
            : base(message)
        {
        }

        public IdentityClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
