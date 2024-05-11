using System.Runtime.Serialization;
using System.Text.Json;

namespace Enqueuer.Identity.Authorization.Tests.Resources;

public static class ResourceHelper
{
    private static readonly string ResourceFolderPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Resources");

    private static readonly JsonSerializerOptions SerializerSettings = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public static string ReadResource(string fileName)
    {
        return File.ReadAllText(Path.Combine(ResourceFolderPath, fileName));
    }

    public static T GetResource<T>(string fileName)
    {
        var json = ReadResource(fileName);
        return JsonSerializer.Deserialize<T>(json, SerializerSettings)
            ?? throw new SerializationException($"Unable to deserialize '{fileName}' to {typeof(T)}.");
    }
}
