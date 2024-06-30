using System;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Identity.Contract.V1.Events
{
    internal class UserInfoUpdatedEvent : IIntegrationEvent
    {
        public UserInfoUpdatedEvent(long userId, string firstName, string? lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id => throw new NotImplementedException();

        public DateTime Timestamp => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public long UserId { get; }

        public string FirstName { get; }

        public string? LastName { get; }
    }
}
