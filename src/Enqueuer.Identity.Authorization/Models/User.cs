namespace Enqueuer.Identity.Authorization.Models;

public class User
{
    public User(long userId, string firstName, string? lastName)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public long UserId { get; }

    public string FirstName { get; }

    public string? LastName { get; }
}
