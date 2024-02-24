using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

public interface IGroupFactory
{
    Group Create(long groupId);
}
