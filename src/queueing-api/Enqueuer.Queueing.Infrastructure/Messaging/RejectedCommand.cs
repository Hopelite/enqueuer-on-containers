using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands;

namespace Enqueuer.Queueing.Infrastructure.Messaging;

public class RejectedCommand : DomainEvent
{
    public RejectedCommand(ICommand rejectedCommand, DomainException domainException)
        : base(rejectedCommand.GroupId, DateTime.UtcNow)
    {
        Command = rejectedCommand;
        Exception = domainException;
    }

    public override string Name => $"Rejected{Command.Name}";

    public ICommand Command { get; }

    public DomainException Exception { get; }

    public override void ApplyTo(Group group)
    {
        Command.Execute(group);
    }
}
