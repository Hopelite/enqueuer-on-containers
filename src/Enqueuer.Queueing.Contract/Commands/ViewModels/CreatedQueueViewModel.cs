namespace Enqueuer.Queueing.Contract.V1.Commands.ViewModels
{
    public class CreatedQueueViewModel
    {
        public CreatedQueueViewModel(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
