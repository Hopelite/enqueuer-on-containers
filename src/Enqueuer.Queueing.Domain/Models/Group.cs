using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Factories;

namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The aggregate root model representing a Telegram group chat with its queues.
/// </summary>
/// <remarks>Provides namespace for queues.</remarks>
public class Group
{
    private readonly Dictionary<string, Queue> _queues;
    private readonly IQueueFactory _queueFactory;

    internal Group(long id, IQueueFactory queueFactory)
    {
        Id = id;
        _queueFactory = queueFactory;
        _queues = new Dictionary<string, Queue>();
    }

    /// <summary>
    /// The unique identifier of the Telegram group.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// The list of the group queues.
    /// </summary>
    public IReadOnlyCollection<Queue> Queues => _queues.Values;

    /// <summary>
    /// Creates queue with the specified <paramref name="queueName"/> in the group.
    /// </summary>
    /// <exception cref="QueueAlreadyExistsException">Thrown if queue with the specified <paramref name="queueName"/> already exists in the group.</exception>
    public void CreateQueue(string queueName)
    {
        if (_queues.ContainsKey(queueName))
        {
            throw new QueueAlreadyExistsException($"Queue '{queueName}' already exists in the chat '{Id}'.");
        }

        var queue = _queueFactory.CreateNew(queueName, groupId: Id);
        _queues.TryAdd(queueName, queue);
    }

    /// <summary>
    /// Deletes queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    public void DeleteQueue(string queueName)
    {
        if (!_queues.Remove(queueName))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }
    }

    /// <summary>
    /// Enqueues a participant with the specified <paramref name="participantId"/> at at the first available position
    /// in the queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    public void EnqueueParticipant(string queueName, long participantId)
    {
        // TODO: method to reconsider using event sourcing
        // Thus we will not only move the logic of the available position calculation to database, but also ensure concurrency
        if (!_queues.TryGetValue(queueName, out var queue))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }

        queue.EnqueueParticipant(participantId);
    }

    /// <summary>
    /// Enqueues a participant with the specified <paramref name="participantId"/> at at the specified <paramref name="position"/>
    /// in the queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    public void EnqueueParticipantAt(string queueName, long participantId, uint position)
    {
        if (!_queues.TryGetValue(queueName, out var queue))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }

        queue.EnqueueParticipantAt(participantId, position);
    }
}
