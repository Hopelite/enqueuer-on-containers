using System.Text.Json;

namespace Enqueuer.Identity.Contract.V1.Models.Utils
{
    internal static class JsonSerialization
    {
        public static JsonSerializerOptions Options => new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }
}
