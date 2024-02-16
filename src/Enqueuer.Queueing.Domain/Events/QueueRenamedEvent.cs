namespace Enqueuer.Queueing.Domain.Events;

public class QueueRenamedEvent : DomainEvent
{
    public QueueRenamedEvent(long queueId, string oldName, string newName)
    {
        QueueId = queueId;
        OldName = oldName;
        NewName = newName;
    }

    public override string Name => nameof(QueueRenamedEvent);

    public long QueueId { get; }

    public string OldName { get; }

    public string NewName { get; }
}
