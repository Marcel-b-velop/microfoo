using Microsoft.Extensions.Caching.Memory;

namespace Host.Register.Adapter;

public class CacheService : ICacheService
{
    private IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public void AddToCache<T>(string key, T value)
    {
        _cache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Cache für 1 Stunde
        });
    }

    public T GetFromCache<T>(string key)
    {
        _cache.TryGetValue(key, out T value);
        return value;
    }
}