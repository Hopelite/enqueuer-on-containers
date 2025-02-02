using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

public interface IGroupFactory
{
    /// <summary>
    /// Creates empty <see cref="Group"/> with specified <paramref name="groupId"/>.
    /// </summary>
    Group Create(long groupId);
}
