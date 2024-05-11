using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds the JWT bearer authentication.
    /// </summary>
    /// <remarks>Requires <see cref="OAuthConfiguration"/> class to be registered and configured.</remarks>
    public static AuthenticationBuilder AddJwtTokenAuthentication(this AuthenticationBuilder builder)
    {
        var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<OAuthConfiguration>>().Value;
        return builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Audience = configuration.Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = configuration.GetSigningKey(),
                ValidIssuer = configuration.Issuer,
                ValidateLifetime = true,
                ValidateAudience = false, // TODO: Ignore for now
            };
        });
    }
}
