using System.Runtime.Serialization;
using System.Text.Json;

namespace Enqueuer.Identity.API.Resources;

public static class ResourceHelper
{
    private static readonly string ResourceFolderPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Resources");

    private static readonly JsonSerializerOptions SerializerSettings = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public static T GetResource<T>(string fileName)
    {
        var json = File.ReadAllText(Path.Combine(ResourceFolderPath, fileName));
        return JsonSerializer.Deserialize<T>(json, SerializerSettings)
            ?? throw new SerializationException($"Unable to deserialize '{fileName}' to {typeof(T)}.");
    }
}
