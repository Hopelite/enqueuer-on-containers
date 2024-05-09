using Enqueuer.Identity.Authorization.OAuth.Signature;
using Microsoft.Extensions.Options;

namespace Enqueuer.Identity.API.Services;

public class SignatureProviderFactory(IServiceProvider serviceProvider) : ISignatureProviderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ValueTask<ITokenSignatureProvider> CreateAsync(CancellationToken cancellationToken)
    {
        var signatureConfiguration = _serviceProvider.GetRequiredService<IOptions<TokenSignatureProviderConfiguration>>();
        return ValueTask.FromResult(new InMemorySignatureProvider(signatureConfiguration.Value) as ITokenSignatureProvider);
    }
}
