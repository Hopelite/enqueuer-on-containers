using System;
using System.Collections.Generic;
using Enqueuer.OAuth.Core.Enums;

namespace Enqueuer.OAuth.Core.Exceptions
{
    /// <summary>
    /// The base exception thrown in case of an authorization error.
    /// </summary>
    public abstract class AuthorizationException : Exception
    {
        protected AuthorizationException(string errorCode)
            : this(errorCode, message: null)
        {
        }

        protected AuthorizationException(string errorCode, string? message)
            : this(errorCode, message, innerException: null)
        {
        }

        protected AuthorizationException(string errorCode, string? message, Exception? innerException)
            : base(message ?? string.Empty, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// A single ASCII error code from the list of error responses.
        /// </summary>
        public string ErrorCode { get; }

        public IDictionary<string, string> GetQueryParameters()
        {
            var parameters = new Dictionary<string, string>()
            {
                { QueryParameter.ErrorResponse.Error, ErrorCode },
            };

            if (!string.IsNullOrWhiteSpace(Message))
            {
                parameters.Add(QueryParameter.ErrorResponse.ErrorDescription, Message);
            }

            return parameters;
        }
    }
}