using Enqueuer.Identity.Contract.V1.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Identity.Contract.V1
{
    public interface IIdentityClient
    {
        Task<AccessToken> GetAccessTokenAsync(CancellationToken cancellationToken);

        Task<bool> CheckAccessAsync(CheckAccessRequest request, CancellationToken cancellationToken);

        Task CreateOrUpdateUserAsync(CreateOrUpdateUserRequest request, CancellationToken cancellationToken);

        Task GrantAccessAsync(GrantAccessRequest request, CancellationToken cancellationToken);

        Task RevokeAccessAsync(RevokeAccessRequest request, CancellationToken cancellationToken);
    }
}
