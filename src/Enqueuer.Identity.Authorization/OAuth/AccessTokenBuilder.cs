using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.Authorization.Validation.Exceptions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Enqueuer.Identity.Authorization.OAuth;

public class AccessTokenBuilder
{
    private readonly OAuthConfiguration _configuration;
    private readonly IScopeValidator _scopeValidator;
    private readonly List<Scope> _scopes = new(); // TODO: change to unique set
    private bool _wereScopesOmitted = false;
    private ITokenSignatureProvider? _signatureProvider;

    private AccessTokenBuilder(OAuthConfiguration configuration, IScopeValidator scopeValidator)
    {
        _configuration = configuration;
        _scopeValidator = scopeValidator;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AccessTokenBuilder"/> class.
    /// </summary>
    public static AccessTokenBuilder CreateBuilder(OAuthConfiguration configuration, IScopeValidator scopeValidator)
    {
        return new AccessTokenBuilder(configuration, scopeValidator);
    }
    
    /// <summary>
    /// Validates and adds <paramref name="scopes"/> to the token.
    /// </summary>
    /// <remarks>Invalid <paramref name="scopes"/> are ommited and the accepted ones are added to token.</remarks>
    public AccessTokenBuilder AddScopes(IEnumerable<Scope> scopes)
    {
        foreach (var scope in scopes)
        {
            try
            {
                _scopeValidator.Validate(scope);
                _scopes.Add(scope);
            }
            catch (ValidationException)
            {
                // Skip invalid scopes
                _wereScopesOmitted = true;
            }
        }

        return this;
    }

    public AccessTokenBuilder SignToken(ITokenSignatureProvider signatureProvider)
    {
        _signatureProvider = signatureProvider;
        return this;
    }

    public async ValueTask<AccessToken> BuildAsync(CancellationToken cancellationToken)
    {
        var scopes = _wereScopesOmitted ? _scopes.Select(s => s.ToString()).ToArray() : null;

        var token = await GenerateJwtTokenAsync(cancellationToken);

        return new AccessToken(token, OAuthConstants.BearerTokenType, TimeSpan.FromSeconds(3600), scopes);
    }

    private async Task<string> GenerateJwtTokenAsync(CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _configuration.Issuer),
        };

        if (_scopes.Count != 0)
        {
            var scopeClaim = string.Join(OAuthConstants.ScopeDelimiter, _scopes);
            claims.Add(new Claim(OAuthConstants.ScopeClaimName, scopeClaim, ClaimValueTypes.String, _configuration.Issuer));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(3600));

        if (_signatureProvider != null)
        {
            return await _signatureProvider.SignAsync(token, cancellationToken);
        }

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
