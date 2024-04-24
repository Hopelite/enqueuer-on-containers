namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The aggregate root model representing a Telegram group chat with its queues.
/// </summary>
public interface IGroupAggregate
{
    /// <summary>
    /// The unique identifier of the Telegram group.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// The list of the group queues.
    /// </summary>
    IReadOnlyCollection<Queue> Queues { get; }

    /// <summary>
    /// Creates queue with the specified <paramref name="queueName"/> in the group.
    /// </summary>
    /// <exception cref="Exceptions.QueueAlreadyExistsException">Thrown if queue with the specified <paramref name="queueName"/> already exists in the group.</exception>
    /// <exception cref="Exceptions.InvalidQueueNameException">Thrown if specified <paramref name="queueName"/> is invalid.</exception>
    void CreateQueue(string queueName);

    /// <summary>
    /// Deletes queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="Exceptions.QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    void DeleteQueue(string queueName);

    /// <summary>
    /// Enqueues a participant with the specified <paramref name="participantId"/> at at the first available position
    /// in the queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="Exceptions.QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    void EnqueueParticipant(string queueName, long participantId);

    /// <summary>
    /// Enqueues a participant with the specified <paramref name="participantId"/> at the specified <paramref name="position"/>
    /// in the queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="Exceptions.QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    void EnqueueParticipantAt(string queueName, long participantId, uint position);

    /// <summary>
    /// Dequeues a participant with the specified <paramref name="participantId"/> from the queue with the specified <paramref name="queueName"/>.
    /// </summary>
    /// <exception cref="Exceptions.QueueDoesNotExistException">Thrown if queue with the specified <paramref name="queueName"/> does not exist in the group.</exception>
    void DequeueParticipant(string queueName, long participantId);
}