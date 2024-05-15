using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.Authorization.Validation.Exceptions;
using Enqueuer.OAuth.Core.Claims;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Enqueuer.Identity.Authorization.OAuth;

public class AccessTokenBuilder
{
    private readonly OAuthConfiguration _configuration;
    private readonly IScopeValidator _scopeValidator;
    private readonly HashSet<string> _scopes = new();
    private bool _wereScopesOmitted = false;
    private ITokenSignatureProvider? _signatureProvider;
    private string? _audience = null;

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
    /// <remarks>Invalid or duplicate <paramref name="scopes"/> are ommited and the accepted ones are added to token.</remarks>
    public AccessTokenBuilder AddScopes(IEnumerable<Scope> scopes)
    {
        foreach (var scope in scopes)
        {
            try
            {
                _scopeValidator.Validate(scope);
                if (!_scopes.Add(scope.Name))
                {
                    _wereScopesOmitted = true;
                }
            }
            catch (ValidationException)
            {
                // Skip invalid scopes
                _wereScopesOmitted = true;
            }
        }

        return this;
    }

    public AccessTokenBuilder AddAudience(string audience)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(audience, nameof(audience));

        _audience = audience;
        return this;
    }

    /// <summary>
    /// Adds the <paramref name="signatureProvider"/> to be used to sign the token.
    /// </summary>
    public AccessTokenBuilder SignToken(ITokenSignatureProvider signatureProvider)
    {
        _signatureProvider = signatureProvider;
        return this;
    }

    public async ValueTask<AccessToken> BuildAsync(CancellationToken cancellationToken)
    {
        var scopes = _wereScopesOmitted ? _scopes.Select(s => s.ToString()).ToArray() : null;

        var token = await GenerateJwtTokenAsync(cancellationToken);

        return new AccessToken(token, TokenTypes.Bearer, _configuration.TokenLifetime, scopes);
    }

    private async Task<string> GenerateJwtTokenAsync(CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _configuration.Issuer),
        };

        if (_scopes.Count != 0)
        {
            var scopeClaim = ScopeClaim.Create(_scopes, _configuration.Issuer);
            claims.Add(scopeClaim);
        }

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(_configuration.TokenLifetime));

        if (_signatureProvider != null)
        {
            return await _signatureProvider.SignAsync(token, cancellationToken);
        }

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
