namespace Enqueuer.Queueing.API.Application.Queries;

public class GetQueueParticipantsQuery : IOperation
{
    public GetQueueParticipantsQuery(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string QueueName { get; }
}
