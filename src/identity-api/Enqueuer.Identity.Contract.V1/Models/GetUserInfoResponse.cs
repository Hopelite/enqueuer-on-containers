using System.Text.Json.Serialization;

namespace Enqueuer.Identity.Contract.V1.Models
{
    internal class GetUserInfoResponse
    {
        [JsonConstructor]
        public GetUserInfoResponse(long userId, string firstName, string? lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public long UserId { get; }

        public string FirstName { get; }

        public string? LastName { get; }
    }
}
