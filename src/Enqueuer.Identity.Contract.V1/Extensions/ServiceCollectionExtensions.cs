using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Caching;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures and registers an instance of the <see cref="IIdentityClient"/>.
        /// </summary>
        /// <remarks>Requires <see cref="IdentityClientOptions"/> class to be registered and configured.</remarks>
        public static IHttpClientBuilder AddIdentityClient(this IServiceCollection services, string name = "Enqueuer Identity Client")
        {
            services.AddSingleton<IAccessTokenCache, InMemoryTokenCache>();
            return services.AddHttpClient<IIdentityClient, IdentityClient>(name, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<IdentityClientOptions>>().Value;
                client.BaseAddress = options.BaseAddress;
                client.Timeout = options.Timeout;
            }).AddPolicyHandler((serviceProvider, _) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<IdentityClientOptions>>().Value;
                return GetRetryPolicy(options);
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IdentityClientOptions options)
            => HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(options.MaxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
