namespace Enqueuer.Queueing.API.Contract.Commands;

public class EnqueueParticipantAtCommand
{
    public required long ParticipantId { get; init; }
}
