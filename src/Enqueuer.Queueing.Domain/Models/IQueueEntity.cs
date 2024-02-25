namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// Defines the queue entity's domain logic.
/// </summary>
internal interface IQueueEntity
{
    void EnqueueParticipant(long participantId);

    void EnqueueParticipantAt(long participantId, uint position);

    void DequeueParticipant(long participantId);
}
