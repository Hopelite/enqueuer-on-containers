using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using System.Collections.Generic;
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
        /// Gets all queues existing within the group..
        /// </summary>
        Task<IReadOnlyCollection<Queue>> GetGroupQueuesAsync(long groupId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the list of participants of the queue.
        /// </summary>
        Task<IReadOnlyCollection<Participant>> GetQueueParticipantsAsync(long groupId, string queueName, CancellationToken cancellationToken);

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
        Task EnqueueParticipantAt(long groupId, string queueName, uint position, EnqueueParticipantAtCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// Dequeues user from the queue with the specified name.
        /// </summary>
        Task DequeueParticipant(long groupId, string queueName, DequeueParticipantCommand command, CancellationToken cancellationToken);
    }
}
