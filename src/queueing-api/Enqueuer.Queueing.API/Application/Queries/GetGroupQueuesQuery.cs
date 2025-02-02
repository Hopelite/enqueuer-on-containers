namespace Enqueuer.Queueing.API.Application.Queries;

public class GetGroupQueuesQuery : IOperation
{
    public GetGroupQueuesQuery(long groupId)
    {
        GroupId = groupId;
    }

    public long GroupId { get; }
}
