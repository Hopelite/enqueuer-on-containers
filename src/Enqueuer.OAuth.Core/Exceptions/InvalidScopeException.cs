using System;

namespace Enqueuer.OAuth.Core.Exceptions
{
    public class InvalidScopeException : AuthorizationException
    {
        private const string InvalidScopeErrorCode = "invalid_scope";
        private const string DefaultErrorDescription = "The requested scope is invalid, unknown, or malformed.";

        public InvalidScopeException()
            : this(DefaultErrorDescription)
        {
        }

        public InvalidScopeException(string? message)
            : base(InvalidScopeErrorCode, message)
        {
        }

        public InvalidScopeException(string? message, Exception? innerException)
            : base(InvalidScopeErrorCode, message, innerException)
        {
        }

        /// <summary>
        /// Creates the <see cref="InvalidScopeException"/> with a human-readable error_description containing the invalid <paramref name="scope"/>.
        /// </summary>
        public static InvalidScopeException FromScope(string scope)
        {
            const string FormattableErrorDescription = "The requested scope '{0}' is invalid, unknown, or malformed.";

            if (string.IsNullOrWhiteSpace(scope))
            {
                return new InvalidScopeException();
            }

            var errorDescription = string.Format(FormattableErrorDescription, scope);
            return new InvalidScopeException(errorDescription);
        }
    }
}