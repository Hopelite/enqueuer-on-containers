namespace Enqueuer.Telegram.Shared.Serialization;

public interface IDataSerializer
{
    string Serialize<T>(T data);
}
