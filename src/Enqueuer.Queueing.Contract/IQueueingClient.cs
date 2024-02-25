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
        /// Creates a queue with the specified name.
        /// </summary>
        Task CreateQueueAsync(long groupId, string queueName, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a queue with the specified name.
        /// </summary>
        Task DeleteGroupQueue(long groupId, string queueName, CancellationToken cancellationToken);
    }
}
