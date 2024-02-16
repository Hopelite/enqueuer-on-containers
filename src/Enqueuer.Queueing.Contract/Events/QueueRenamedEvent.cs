namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueRenamedEvent : EventBase
    {
        public QueueRenamedEvent(int queueId, string oldName, string newName)
        {
            QueueId = queueId;
            OldName = oldName;
            NewName = newName;
        }

        public int QueueId { get; }

        public string OldName { get; }

        public string NewName { get; }
    }
}
