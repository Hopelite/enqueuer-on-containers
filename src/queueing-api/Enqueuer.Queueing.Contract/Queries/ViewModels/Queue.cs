namespace Enqueuer.Queueing.Contract.V1.Queries.ViewModels
{
    public class Queue
    {
        public Queue(long groupId, string name)
        {
            Name = name;
            GroupId = groupId;
        }

        public long GroupId { get; }

        public string Name { get; }
    }
}
