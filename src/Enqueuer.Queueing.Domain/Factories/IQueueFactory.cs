using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

/// <summary>
/// Defines methods to ease complex <see cref="Queue"/> aggregate creation.
/// </summary>
public interface IQueueFactory
{
    Queue CreateNew(int id, string name, long locationId);

    Queue Create(int id, string name, long locationId, IEnumerable<Participant> participants);
}
