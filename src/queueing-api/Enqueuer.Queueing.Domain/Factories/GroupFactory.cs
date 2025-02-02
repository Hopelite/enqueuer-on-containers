using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

public class GroupFactory : IGroupFactory
{
    public Group Create(long groupId)
    {
        return new Group(groupId);
    }
}
