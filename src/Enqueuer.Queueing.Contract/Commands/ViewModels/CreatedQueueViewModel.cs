namespace Enqueuer.Queueing.Contract.V1.Commands.ViewModels
{
    public class CreatedQueueViewModel
    {
        public CreatedQueueViewModel(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
