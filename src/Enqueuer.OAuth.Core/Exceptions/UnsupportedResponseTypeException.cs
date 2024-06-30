using System;

namespace Enqueuer.OAuth.Core.Exceptions
{
    public class UnsupportedResponseTypeException : AuthorizationException
    {
        private const string UnsupportedErrorCode = "unsupported_response_type";

        public UnsupportedResponseTypeException()
            : base(UnsupportedErrorCode)
        {
        }

        public UnsupportedResponseTypeException(string? message)
            : base(UnsupportedErrorCode, message)
        {
        }

        public UnsupportedResponseTypeException(string? message, Exception? innerException)
            : base(UnsupportedErrorCode, message, innerException)
        {
        }
    }
}