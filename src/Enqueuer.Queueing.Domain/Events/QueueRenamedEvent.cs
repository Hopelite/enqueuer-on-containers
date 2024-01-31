namespace Enqueuer.Queueing.Domain.Events;

public class QueueRenamedEvent : DomainEvent
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
