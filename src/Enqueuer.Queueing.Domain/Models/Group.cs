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
    /// Creates queue with the specified <paramref name="queueName"/> in the chat.
    /// </summary>
    /// <exception cref="QueueAlreadyExistsException">Thrown if queue with the specified <paramref name="queueName"/> already exists in the chat.</exception>
    public void CreateQueue(string queueName)
    {
        if (_queues.ContainsKey(queueName))
        {
            throw new QueueAlreadyExistsException($"Queue '{queueName}' already exists in the chat '{Id}'.");
        }

        var queue = _queueFactory.CreateNew(queueName, groupId: Id);
        _queues.TryAdd(queueName, queue);
    }

    public void DeleteQueue(string queueName)
    {
        if (!_queues.Remove(queueName))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }
    }
}
