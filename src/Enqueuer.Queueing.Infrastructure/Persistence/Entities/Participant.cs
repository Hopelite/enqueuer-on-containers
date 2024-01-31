namespace Enqueuer.Queueing.Infrastructure.Persistence.Entities;

public class Participant
{
    public long Id { get; set; }

    public uint Number { get; set; }

    public int QueueId { get; set; }

    public Queue Queue { get; set; }
}
