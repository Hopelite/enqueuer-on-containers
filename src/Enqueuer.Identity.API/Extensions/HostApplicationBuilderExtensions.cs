using Azure.Identity;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Authorization;
using Enqueuer.Identity.Authorization.Configuration;
using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Grants.Credentials;
using Enqueuer.Identity.Authorization.Grants.Validation;
using Enqueuer.Identity.Authorization.OAuth;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Enqueuer.Identity.API.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>()
                        .AddDbContext<IdentityContext>(options =>
                            options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDB")));

        ConfigureOAuth(builder);

        return builder;
    }

    private static WebApplicationBuilder ConfigureOAuth(this WebApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration.GetRequiredSection("KeyVault").GetRequiredUri("Url");

        builder.Services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddSecretClient(keyVaultUri)
                .WithCredential(new DefaultAzureCredential());
        });

        builder.Services.AddTransient<IOAuthService, OAuthService>()
                        .AddTransient<IAuthorizationGrantValidator, AuthorizationGrantValidator>()
                        .AddTransient<IAuthorizationContext, AuthorizationContext>()
                        .AddTransient<IScopeValidator, ScopeValidator>()
                        .AddSingleton<IClientCredentialsStorage, AzureKeyVaultStorage>()
                        .AddTransient<ISignatureProviderFactory, SignatureProviderFactory>()
                        .Configure<OAuthConfiguration>(builder.Configuration.GetRequiredSection("OAuth"))
                        .Configure<TokenSignatureProviderConfiguration>(builder.Configuration.GetRequiredSection("TokenSign"));

        var signatureConfiguration = builder.Services.BuildServiceProvider()
            .GetRequiredService<IOptions<TokenSignatureProviderConfiguration>>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(signatureConfiguration.Value.EncodedKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,  // Remove delay of token when expire
            };
        });

        return builder;
    }
}
