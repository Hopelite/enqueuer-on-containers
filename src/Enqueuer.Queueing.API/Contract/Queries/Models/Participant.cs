namespace Enqueuer.Queueing.API.Contract.Queries.Models;

public class Participant
{
    public required long Id { get; init; }

    public required int QueueId { get; init; }

    public required uint Position { get; init; }
}
