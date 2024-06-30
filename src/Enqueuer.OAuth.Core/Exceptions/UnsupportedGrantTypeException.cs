using System;

namespace Enqueuer.OAuth.Core.Exceptions
{
    public class UnsupportedGrantTypeException : AuthorizationException
    {
        private const string UnsupportedGrantTypeErrorCode = "unsupported_grant_type";
        private const string DefaultErrorDescription = "The authorization grant type is not supported by the authorization server.";

        public UnsupportedGrantTypeException()
            : this(DefaultErrorDescription)
        {
        }

        public UnsupportedGrantTypeException(string? message)
            : base(UnsupportedGrantTypeErrorCode, message)
        {
        }

        public UnsupportedGrantTypeException(string? message, Exception? innerException)
            : base(UnsupportedGrantTypeErrorCode, message, innerException)
        {
        }

        /// <summary>
        /// Creates the <see cref="UnsupportedGrantTypeException"/> with a human-readable error_description containing the unsupported <paramref name="grantType"/>.
        /// </summary>
        public static UnsupportedGrantTypeException FromGrantType(string grantType)
        {
            const string FormattableErrorDescription = "The authorization grant type '{0}' is not supported by the authorization server.";

            var errorDescription = string.Format(FormattableErrorDescription, grantType);
            return new UnsupportedGrantTypeException(errorDescription);
        }
    }
}