using Newtonsoft.Json;

namespace Enqueuer.Telegram.Shared.Serialization;

public class JsonDataSerializer : IDataSerializer
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore,
    };

    public string Serialize<T>(T data)
    {
        return JsonConvert.SerializeObject(data, SerializerSettings);
    }
}
