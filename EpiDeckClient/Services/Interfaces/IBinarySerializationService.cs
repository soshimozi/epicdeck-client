namespace EpiDeckClient.Services.Interfaces
{
    public interface IBinarySerializationService
    {
        T Deserialize<T>(byte[] buffer) where T : new();
    }

}