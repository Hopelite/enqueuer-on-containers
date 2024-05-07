using Enqueuer.Identity.Authorization.Configuration;
using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.Authorization.Validation.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Enqueuer.Identity.Authorization.OAuth;

public class AccessTokenBuilder
{
    private const string BearerTokenType = "bearer";

    private readonly OAuthConfiguration _configuration;
    private readonly IScopeValidator _scopeValidator;
    private readonly List<Scope> _scopes = new();
    private bool _wereScopesOmitted = false;
    private SigningCredentials? _signature;

    private AccessTokenBuilder(OAuthConfiguration configuration, IScopeValidator scopeValidator)
    {
        _configuration = configuration;
        _scopeValidator = scopeValidator;
    }

    public static AccessTokenBuilder CreateBuilder(OAuthConfiguration configuration, IScopeValidator scopeValidator)
    {
        return new AccessTokenBuilder(configuration, scopeValidator);
    }

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
        _signature = signatureProvider.GenerateSignature();
        return this;
    }

    public AccessToken Build()
    {
        var scopes = _wereScopesOmitted ? _scopes.Select(s => s.ToString()).ToArray() : null;

        var token = GenerateJwtToken();

        return new AccessToken(token, BearerTokenType, TimeSpan.FromSeconds(_configuration.TokenLifetime), scopes);
    }

    private string GenerateJwtToken()
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (_scopes.Count != 0)
        {
            var scopeClaim = string.Join(Constants.ScopeDelimiter, _scopes);
            claims.Add(new Claim(Constants.ScopeClaimName, scopeClaim));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(_configuration.TokenLifetime),
            signingCredentials: _signature
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
