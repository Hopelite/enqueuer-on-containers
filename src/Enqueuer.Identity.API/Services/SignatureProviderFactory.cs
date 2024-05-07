using Enqueuer.Identity.Authorization.OAuth.Signature;

namespace Enqueuer.Identity.API.Services;

public class SignatureProviderFactory(IServiceProvider serviceProvider) : ISignatureProviderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ValueTask<ITokenSignatureProvider> CreateAsync(CancellationToken cancellationToken)
    {
        const string Key = "MYBIGGESTKEYPOSSIBLEJUSTLOOKATTHISDUDE";

        return ValueTask.FromResult(new TokenSignatureProvider(new TokenSignatureProviderConfiguration(Key)) as ITokenSignatureProvider);
    }
}
