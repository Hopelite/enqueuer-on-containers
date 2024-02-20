using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

/// <summary>
/// Defines methods to ease complex <see cref="Queue"/> aggregate creation.
/// </summary>
public interface IQueueFactory
{
    Queue CreateNew(string name, long groupId);
    
    Queue CreateNew(long id, string name, long groupId);

    Queue Create(long id, string name, long groupId, IEnumerable<Participant> participants);
}
