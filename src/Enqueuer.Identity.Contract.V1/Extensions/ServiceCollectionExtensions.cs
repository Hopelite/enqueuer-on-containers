using System;
using System.Net.Http;
using Enqueuer.Identity.Contract.V1;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

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

            // TODO: Add token refresher here
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IdentityClientOptions options)
            => HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(options.MaxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
