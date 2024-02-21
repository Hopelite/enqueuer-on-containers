using System.Collections.Generic;

namespace Enqueuer.Queueing.Contract.V1.Queries.ViewModels
{
    public class Queue
    {
        public Queue(long groupId, string name, IReadOnlyCollection<Participant> participants)
        {
            Name = name;
            GroupId = groupId;
            Participants = participants;
        }

        public long GroupId { get; }

        public string Name { get; }

        public IReadOnlyCollection<Participant> Participants { get; }
    }
}
