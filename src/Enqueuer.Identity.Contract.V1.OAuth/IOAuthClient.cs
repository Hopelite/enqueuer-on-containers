using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Identity.Contract.V1.OAuth.Models;
using Enqueuer.OAuth.Core.Tokens;

namespace Enqueuer.Identity.Contract.V1.OAuth
{
    public interface IOAuthClient
    {
        Task<AccessToken> GetAccessTokenAsync(IAccessTokenRequest request, CancellationToken cancellationToken);
    }
}
