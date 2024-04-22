using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public interface ICommand
{
    long GroupId { get; }

    string Name { get; }

    void Execute(Group group);
}
