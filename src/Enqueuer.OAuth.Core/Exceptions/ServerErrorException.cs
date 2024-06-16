using System;

namespace Enqueuer.OAuth.Core.Exceptions
{
    public class ServerErrorException : AuthorizationException
    {
        private const string ServerErrorCode = "server_error";
        private const string ErrorDescription = "The authorization server encountered an unexpected condition that prevented it from fulfilling the request.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerErrorException"/> class.
        /// </summary>
        /// <remarks>The inner exception must not be displayed to the client.</remarks>
        public ServerErrorException(Exception? innerException)
            : base(ServerErrorCode, ErrorDescription, innerException)
        {
        }
    }
}