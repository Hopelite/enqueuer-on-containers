namespace Enqueuer.Queueing.API.Application.Commands;

public class EnqueueParticipantToCommand : IOperation
{
    public EnqueueParticipantToCommand(long groupId, string queueName, long participantId, uint position)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
        Position = position;
    }

    public long GroupId { get; }

    public string QueueName { get; }

    public long ParticipantId { get; }

    public uint Position { get; }
}
