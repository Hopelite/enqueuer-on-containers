using Enqueuer.Identity.Contract.V1.Models;

namespace Enqueuer.Identity.Contract.V1.Caching
{
    public interface IAccessTokenCache
    {
        AccessToken? GetAccessToken();
        
        void SetAccessToken(AccessToken accessToken);
    }

    public class InMemoryTokenCache : IAccessTokenCache
    {
        private AccessToken? _accessToken;

        public AccessToken? GetAccessToken()
        {
            return _accessToken;
        }

        public void SetAccessToken(AccessToken accessToken)
        {
            _accessToken = accessToken;
        }
    }
}
