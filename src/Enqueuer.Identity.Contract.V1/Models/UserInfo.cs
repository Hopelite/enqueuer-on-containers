using System.Text.Json.Serialization;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class UserInfo
    {
        [JsonConstructor]
        public UserInfo(long userId, string firstName, string? lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public long UserId { get; }

        public string FirstName { get; }

        public string? LastName { get; }

        /// <summary>
        /// Gets the full name of the user.
        /// </summary>
        public string FullName => string.IsNullOrWhiteSpace(LastName) ? FirstName : $"{FirstName} {LastName}";
    }
}
