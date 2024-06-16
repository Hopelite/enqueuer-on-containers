using System;

namespace Enqueuer.OAuth.Core.Exceptions
{
    public class InvalidRequestException : AuthorizationException
    {
        private const string InvalidRequestErrorCode = "invalid_request";

        public InvalidRequestException()
            : base(InvalidRequestErrorCode)
        {
        }

        public InvalidRequestException(string? message)
            : base(InvalidRequestErrorCode, message)
        {
        }

        public InvalidRequestException(string? message, Exception? innerException)
            : base(InvalidRequestErrorCode, message, innerException)
        {
        }
    }
}