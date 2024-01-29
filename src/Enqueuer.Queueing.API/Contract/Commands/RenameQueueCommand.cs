namespace Enqueuer.Queueing.API.Contract.Commands;

public class RenameQueueCommand
{
    public required string NewQueueName { get; init; }
}
