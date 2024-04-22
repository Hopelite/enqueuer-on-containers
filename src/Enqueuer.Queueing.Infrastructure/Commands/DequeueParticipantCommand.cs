using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public class DequeueParticipantCommand : ICommand
{
    public DequeueParticipantCommand(long groupId, string queueName, long participantId)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public long GroupId { get; }

    public string Name => nameof(DequeueParticipantCommand);

    public string QueueName { get; }

    public long ParticipantId { get; }

    public void Execute(Group group)
    {
        group.DequeueParticipant(QueueName, ParticipantId);
    }
}
