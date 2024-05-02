using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Grants.Validation;
using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.Validation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Enqueuer.Identity.Authorization;

public class OAuthService : IOAuthService
{
    private const string BearerTokenType = "bearer";
    private const int TokenExpirationInSeconds = 3600;

    private readonly IAuthorizationGrantValidator _grantValidator;
    private readonly IScopeValidator _scopeValidator;

    public OAuthService(IAuthorizationGrantValidator grantValidator, IScopeValidator scopeValidator)
    {
        _grantValidator = grantValidator;
        _scopeValidator = scopeValidator;
    }

    public AccessToken GetAccessTokenAsync(IAuthorizationGrant grant, IReadOnlyCollection<Scope> scopes)
    {
        _grantValidator.Validate(grant);

        var acceptedScopes = new List<string>(scopes.Count);
        foreach (var scope in scopes)
        {
            try
            {
                _scopeValidator.Validate(scope);
                acceptedScopes.Add(scope.Name);
            }
            catch (Exception)
            {
                // Skip invalid scopes
            }
        }

        // If some scopes where omitted
        var shouldProvideScopes = acceptedScopes.Count != scopes.Count;

        var token = GenerateJwtToken(acceptedScopes);
        return shouldProvideScopes
            ? new AccessToken(token, BearerTokenType, TimeSpan.FromSeconds(TokenExpirationInSeconds), acceptedScopes.ToArray())
            : new AccessToken(token, BearerTokenType, TimeSpan.FromSeconds(TokenExpirationInSeconds));
    }

    private string GenerateJwtToken(IEnumerable<string> scopes)
    {
        const long GroupId = 442953378;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MYBIGGESTKEYPOSSIBLEJUSTLOOKATTHISDUDE"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "1"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("scope", string.Join(' ', scopes)),
            new Claim("queue_access", "Test"),
            new Claim("group_access", GroupId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "EnqueuerIdentity",
            audience: "apis",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Adjust token expiration as needed
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
