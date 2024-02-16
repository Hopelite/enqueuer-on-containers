namespace Enqueuer.Queueing.Infrastructure.Persistence.Entities;

public class Participant
{
    public long Id { get; set; }

    public uint Number { get; set; }

    public long QueueId { get; set; }

    public Queue Queue { get; set; }
}
