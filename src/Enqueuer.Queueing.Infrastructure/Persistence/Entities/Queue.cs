namespace Enqueuer.Queueing.Infrastructure.Persistence.Entities;

public class Queue
{
    public int Id { get; set; }

    public string Name { get; set; }

    public long LocationId { get; set; }

    public IEnumerable<Participant> Participants { get; set; }
}
