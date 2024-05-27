using Azure.Identity;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Authorization;
using Enqueuer.Identity.Authorization.OAuth;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System.Text.Json;

namespace Enqueuer.Identity.API.Extensions;

public static class WebApplicationBuilderExtensions
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

        //builder.Services.AddTransient<IOAuthService, OAuthService>()
        //                .AddTransient<IAuthorizationGrantValidator, AuthorizationGrantValidator>()
        //                .AddTransient<IAuthorizationContext, AuthorizationContext>()
        //                .AddTransient<IScopeValidator, ScopeValidator>()
        //                .AddSingleton<IClientCredentialsStorage, AzureKeyVaultStorage>()
        //                .AddTransient<ISignatureProviderFactory, SignatureProviderFactory>()
        builder.Services.Configure<OAuthConfiguration>(builder.Configuration.GetRequiredSection("OAuth"));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtTokenAuthentication();

        return builder;
    }
}
