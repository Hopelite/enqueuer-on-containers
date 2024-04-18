using Enqueuer.Queueing.Contract.V1.Commands;
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
        Task DeleteQueueAsync(long groupId, string queueName, CancellationToken cancellationToken);

        /// <summary>
        /// Enqueues participant to the queue with the specified name at the first available position.
        /// </summary>
        Task EnqueueParticipant(long groupId, string queueName, EnqueueParticipantCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// Enqueues participant to the queue with the specified name at the specified position.
        /// </summary>
        Task EnqueueParticipantTo(long groupId, string queueName, uint position, EnqueueParticipantToCommand command, CancellationToken cancellationToken);
    }
}
