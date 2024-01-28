namespace Enqueuer.Queueing.API.Application.Commands;

public class RenameQueueCommand
{
    public required string NewQueueName { get; init; }
}
