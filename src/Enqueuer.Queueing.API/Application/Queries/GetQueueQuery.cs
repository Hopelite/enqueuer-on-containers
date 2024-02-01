using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Queries;

public class GetQueueQuery : IRequest<Queue>
{
    public GetQueueQuery(int id)
    {
        Id = id;
    }

    public int Id { get; }
}
