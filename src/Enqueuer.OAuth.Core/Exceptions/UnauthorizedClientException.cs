namespace Enqueuer.OAuth.Core.Exceptions
{
    public class UnauthorizedClientException : AuthorizationException
    {
        private const string ServerErrorCode = "unauthorized_client";
        private const string ErrorDescription = "The client is not authorized to request an authorization code using this method.";

        public UnauthorizedClientException()
            : base(ServerErrorCode, ErrorDescription)
        {
        }
    }
}