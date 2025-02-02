namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class DequeueParticipantCommand
    {
        public DequeueParticipantCommand(long participantId)
        {
            ParticipantId = participantId;
        }

        public long ParticipantId { get; }
    }
}
