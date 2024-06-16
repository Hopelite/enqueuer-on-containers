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
    }
}