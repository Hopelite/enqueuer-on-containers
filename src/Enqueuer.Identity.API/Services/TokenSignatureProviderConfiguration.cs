using System.Text;

namespace Enqueuer.Identity.API.Services;

public class TokenSignatureProviderConfiguration
{
    public TokenSignatureProviderConfiguration(string key)
    {
        Key = Encoding.UTF8.GetBytes(key);
    }

    public byte[] Key { get; }
}
