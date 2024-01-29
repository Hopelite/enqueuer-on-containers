using System.Collections.Generic;

namespace Enqueuer.Queueing.Contract.V1.Queries.Models
{
    public class Queue
    {
        public Queue(int id, string name, long locationId, IReadOnlyCollection<Participant> participants)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            Participants = participants;
        }

        public int Id { get; }

        public string Name { get; }

        public long LocationId { get; }

        public IReadOnlyCollection<Participant> Participants { get; }
    }
}
