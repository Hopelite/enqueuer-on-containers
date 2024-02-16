namespace Enqueuer.Queueing.Contract.V1.Commands.ViewModels
{
    public class EnqueuedParticipantViewModel
    {
        public EnqueuedParticipantViewModel(uint position)
        {
            Position = position;
        }

        public uint Position { get; }
    }
}
