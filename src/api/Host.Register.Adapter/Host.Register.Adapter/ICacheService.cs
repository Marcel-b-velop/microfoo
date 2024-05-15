namespace com.b_velop.microfe;

public interface ICacheService
{
    void AddToCache<T>(string key, T value);
    T GetFromCache<T>(string key);
}