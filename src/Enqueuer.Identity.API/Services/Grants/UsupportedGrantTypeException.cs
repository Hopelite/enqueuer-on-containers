namespace Enqueuer.Identity.API.Services.Grants;

public class UsupportedGrantTypeException : Exception
{
    public UsupportedGrantTypeException(string grantType)
        : this(grantType, null)
    {
    }

    public UsupportedGrantTypeException(string grantType, string? message)
        : this(grantType, message, null)
    {
    }

    public UsupportedGrantTypeException(string grantType, string? message, Exception? innerException)
        : base(message, innerException)
    {
        GrantType = grantType;
    }

    public string GrantType { get; }
}
