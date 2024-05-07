namespace Enqueuer.Identity.Authorization.OAuth.Signature;

public interface ISignatureProviderFactory
{
    ValueTask<ITokenSignatureProvider> CreateAsync(CancellationToken cancellationToken);
}
