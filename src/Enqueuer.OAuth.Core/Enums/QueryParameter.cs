namespace Enqueuer.OAuth.Core.Enums
{
    /// <summary>
    /// The known query parameter for requests and responses.
    /// </summary>
    public static class QueryParameter
    {
        public static class AuthorizationResponse
        {
            public const string AuthorizationCode = "code";

            public const string State = "state";
        }

        public static class ErrorResponse
        {
            public const string Error = "error";

            /// <summary>
            /// Human-readable ASCII [USASCII] text providing additional information,
            /// used to assist the client developer in understanding the error that occurred.
            /// </summary>
            /// <remarks>Values for the "error_description" parameter MUST NOT characters outside the set %x20-21 / %x23-5B / %x5D-7E.</remarks>
            public const string ErrorDescription = "error_description";

            /// <summary>
            /// A URI identifying a human-readable web page with information about the error,
            /// used to provide the client developer with additional information about the error.
            /// </summary>
            /// <remarks>
            /// Values for the "error_uri" parameter MUST conform to the URI-reference syntax
            /// and thus MUST NOT include characters outside the set %x21 / %x23-5B / %x5D-7E.
            /// </remarks>
            public const string ErrorUri = "error_uri";

            /// <summary>
            /// REQUIRED if a "state" parameter was present in the client authorization request.The exact value received from the client.
            /// </summary>
            public const string State = "state";
        }
    }
}