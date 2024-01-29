namespace Enqueuer.Queueing.API.Contract.Commands;

public class CreateQueueCommand
{
    public required string QueueName { get; init; }
}
