namespace Enqueuer.Queueing.API.Contract.Commands;

public class EnqueueParticipantCommand
{
    public required long ParticipantId { get; init; }
}
