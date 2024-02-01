namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class EnqueueParticipantCommand
    {
        public EnqueueParticipantCommand(long participantId)
        {
            ParticipantId = participantId;
        }

        public long ParticipantId { get; }
    }
}
