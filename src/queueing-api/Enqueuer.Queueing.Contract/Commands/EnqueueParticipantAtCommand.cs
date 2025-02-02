namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class EnqueueParticipantAtCommand
    {
        public EnqueueParticipantAtCommand(long participantId)
        {
            ParticipantId = participantId;
        }

        public long ParticipantId { get; }
    }
}
