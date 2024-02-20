using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Factories;

public class QueueFactory : IQueueFactory
{
    public Queue CreateNew(string name, long groupId)
    {
        throw new NotImplementedException();
    }

    public Queue CreateNew(long id, string name, long groupId)
    {
        var queue = new Queue(id, name, groupId);
        queue.AddDomainEvent(new QueueCreatedEvent(id, name, groupId));
        return queue;
    }

    public Queue Create(long id, string name, long groupId, IEnumerable<Participant> participants)
    {
        var queue = new Queue(id, name, groupId);
        foreach (var participant in participants)
        {
            queue.EnqueueParticipant(participant.Id, participant.Position.Number);
        }

        return queue;
    }
}
