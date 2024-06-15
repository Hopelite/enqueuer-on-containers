namespace Enqueuer.Identity.Contract.V1.Models
{
    public class UserInfo
    {
        internal UserInfo(long userId, string firstName, string? lastName, Metadata metadata)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Metadata = metadata;
        }

        public long UserId { get; }

        public string FirstName { get; }

        public string? LastName { get; }

        public Metadata Metadata { get; }
    }
}
