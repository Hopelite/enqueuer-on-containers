namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand
{
    public required string QueueName { get; init; }
}
