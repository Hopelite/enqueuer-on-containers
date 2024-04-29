using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Identity.Contract.V1
{
    public interface IIdentityClient
    {
        Task AuthorizeAsync(CancellationToken cancellationToken);
    }
}
