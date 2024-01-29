namespace Enqueuer.Queueing.API.Contract.Queries.Models;

public class Queue
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required long LocationId { get; init; }

    public required IReadOnlyCollection<Participant> Participants { get; init; }
}
