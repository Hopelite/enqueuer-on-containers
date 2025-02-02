using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public class EnqueueParticipantCommand : ICommand
{
    public EnqueueParticipantCommand(long groupId, string queueName, long participantId)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public long GroupId { get; }

    public string Name => nameof(EnqueueParticipantCommand);

    public string QueueName { get; }

    public long ParticipantId { get; }

    public void Execute(Group group)
    {
        group.EnqueueParticipant(QueueName, ParticipantId);
    }
}
