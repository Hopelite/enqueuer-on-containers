namespace Enqueuer.Identity.OAuth.JWT;

public class JwtTokenIssuingConfiguration
{
    public required string Issuer { get; init; }

    public required TimeSpan TokenLifetime { get; init; }
}
