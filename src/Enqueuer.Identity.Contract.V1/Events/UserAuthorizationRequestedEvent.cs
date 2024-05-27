using Enqueuer.EventBus.Abstractions;
using System;

namespace Enqueuer.Identity.Contract.V1.Events
{
    public class UserAuthorizationRequestedEvent : IIntegrationEvent
    {
        public UserAuthorizationRequestedEvent(string? state, Uri identityProviderRedirectUri, long userId)
            : this(Guid.NewGuid(), DateTime.UtcNow, state, identityProviderRedirectUri, userId)
        {
        }

        public UserAuthorizationRequestedEvent(
            Guid id,
            DateTime timestamp, string? state,
            Uri identityProviderRedirectUri,
            long userId)
        {
            Id = id;
            Timestamp = timestamp;
            State = state;
            IdentityProviderRedirectUri = identityProviderRedirectUri;
            UserId = userId;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public string? State { get; }

        public Uri IdentityProviderRedirectUri { get; }

        public long UserId { get; }

        // TODO: possibly include user ui lang
    }
}
