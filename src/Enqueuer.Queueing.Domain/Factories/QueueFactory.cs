using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

public class QueueFactory : IQueueFactory
{
    public Queue Create(int id, string name, long locationId)
    {
        return new Queue(id, name, locationId);
    }

    public Queue Create(int id, string name, long locationId, IEnumerable<Participant> participants)
    {
        var queue = new Queue(id, name, locationId);
        foreach (var participant in participants)
        {
            queue.EnqueueParticipant(participant.Id, participant.Position.Number);
        }

        return queue;
    }
}
