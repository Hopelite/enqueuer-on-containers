using System.Collections.Generic;

namespace Enqueuer.Queueing.Contract.V1.Queries.ViewModels
{
    public class Queue
    {
        public Queue(long id, string name, long groupId, IReadOnlyCollection<Participant> participants)
        {
            Id = id;
            Name = name;
            GroupId = groupId;
            Participants = participants;
        }

        public long Id { get; }

        public string Name { get; }

        public long GroupId { get; }

        public IReadOnlyCollection<Participant> Participants { get; }
    }
}
