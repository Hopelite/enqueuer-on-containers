namespace Enqueuer.Queueing.API.Application.Commands;

public class DequeueParticipantCommand : IOperation
{
    public DequeueParticipantCommand(long groupId, string queueName, long participantId)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public long GroupId { get; }

    public string QueueName { get; }

    public long ParticipantId { get; }
}
