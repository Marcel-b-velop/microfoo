namespace Host.Register.Adapter;

public interface ICacheService
{
    void AddToCache<T>(string key, T value);
    T GetFromCache<T>(string key);
}