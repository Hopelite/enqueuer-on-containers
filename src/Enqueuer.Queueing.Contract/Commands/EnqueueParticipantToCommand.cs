namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class EnqueueParticipantToCommand
    {
        public EnqueueParticipantToCommand(long participantId)
        {
            ParticipantId = participantId;
        }

        public long ParticipantId { get; }
    }
}
