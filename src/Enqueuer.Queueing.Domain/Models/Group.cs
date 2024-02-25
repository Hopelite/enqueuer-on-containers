using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;

namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The aggregate root model representing a Telegram group chat with its queues.
/// </summary>
/// <remarks>Provides namespace for queues.</remarks>
public class Group : Entity, IGroupAggregate
{
    internal readonly Dictionary<string, Queue> _queues = new();

    /// <summary>
    /// Initializes empty group without queues.
    /// </summary>
    /// <remarks>Made internal to restrict creation via factory only.</remarks>
    internal Group(long id)
    {
        Id = id;
    }

    public long Id { get; }

    public IReadOnlyCollection<Queue> Queues => _queues.Values;

    public void CreateQueue(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            throw new InvalidQueueNameException("Queue name can't be null, empty or a whitespace.");
        }

        if (queueName.Length > QueueRestrictions.MaxNameLength)
        {
            throw new InvalidQueueNameException($"Queue name can't be longer than {QueueRestrictions.MaxNameLength} symbols.");
        }

        if (_queues.TryAdd(queueName, new Queue(Id, queueName)))
        {
            throw new QueueAlreadyExistsException($"Queue '{queueName}' already exists in the chat '{Id}'.");
        }

        AddDomainEvent(new QueueCreatedEvent(Id, queueName, DateTime.UtcNow));
    }

    public void DeleteQueue(string queueName)
    {
        if (!_queues.Remove(queueName))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }

        AddDomainEvent(new QueueDeletedEvent(Id, queueName, DateTime.UtcNow));
    }

    public void EnqueueParticipant(string queueName, long participantId)
    {
        var queue = GetExistingQueueOrThrow(queueName);
        queue.EnqueueParticipant(participantId);
    }

    public void EnqueueParticipantAt(string queueName, long participantId, uint position)
    {
        var queue = GetExistingQueueOrThrow(queueName);
        queue.EnqueueParticipantAt(participantId, position);
    }

    public void DequeueParticipant(string queueName, long participantId)
    {
        if (!_queues.TryGetValue(queueName, out var queue))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }

        queue.DequeueParticipant(participantId);
    }

    /// <summary>
    /// Applies <paramref name="domainEvent"/> to this group.
    /// </summary>
    public void Apply(DomainEvent domainEvent)
    {
        domainEvent.ApplyTo(this);
    }

    private Queue GetExistingQueueOrThrow(string queueName)
    {
        if (!_queues.TryGetValue(queueName, out var queue))
        {
            throw new QueueDoesNotExistException($"Queue '{queueName}' does not exist in the group '{Id}'.");
        }

        return queue;
    }
}
