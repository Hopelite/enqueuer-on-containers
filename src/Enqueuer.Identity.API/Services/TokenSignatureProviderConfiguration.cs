using System.Text;

namespace Enqueuer.Identity.API.Services;

public class TokenSignatureProviderConfiguration
{
    public string Key { get; init; } = null!;

    public byte[] EncodedKey => Encoding.UTF8.GetBytes(Key);
}
