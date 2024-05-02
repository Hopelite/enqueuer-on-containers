using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace Enqueuer.Identity.Authorization.Tests.Resources;

public static class ResourceHelper
{
    private static readonly string ResourceFolderPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Resources");

    private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    public static string ReadResource(string fileName)
    {
        return File.ReadAllText(Path.Combine(ResourceFolderPath, fileName));
    }

    public static T ReadResource<T>(string fileName)
    {
        var json = ReadResource(fileName);
        return JsonConvert.DeserializeObject<T>(json, SerializerSettings)
            ?? throw new SerializationException($"Unable to deserialize '{fileName}' to {typeof(T)}.");
    }
}
