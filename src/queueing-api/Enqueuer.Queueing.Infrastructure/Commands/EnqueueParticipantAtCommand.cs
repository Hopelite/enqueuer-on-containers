using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public class EnqueueParticipantAtCommand : ICommand
{
    public EnqueueParticipantAtCommand(long groupId, string queueName, long participantId, uint position)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
        Position = position;
    }

    public long GroupId { get; }

    public string Name => nameof(EnqueueParticipantAtCommand);

    public string QueueName { get; }

    public long ParticipantId { get; }

    public uint Position { get; }

    public void Execute(Group group)
    {
        group.EnqueueParticipantAt(QueueName, ParticipantId, Position);
    }
}
