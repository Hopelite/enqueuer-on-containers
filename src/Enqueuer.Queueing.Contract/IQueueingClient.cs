using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Commands.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Queueing.Contract.V1
{
    /// <summary>
    /// Defines the way for clients to communicate with Queueing API.
    /// </summary>
    public interface IQueueingClient
    {
        /// <summary>
        /// Creates a queue with the specified name and returns its unique identifier.
        /// </summary>
        Task<CreatedQueueViewModel> CreateQueueAsync(CreateQueueCommand command, CancellationToken cancellationToken);
    }
}
