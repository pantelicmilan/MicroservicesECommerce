using AuthService.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Services;

public enum CacheType
{
    UserVerificationCode
}
public class MemoryCachingHandler : IMemoryCachingHandler
{
    private readonly IMemoryCache _memoryCache;
    public MemoryCachingHandler(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void SetCache<T>(CacheType cacheType, T cacheValue, string cacheKey) 
    {
        uint expirationTimespanInSeconds = 1200;
        if(cacheType == CacheType.UserVerificationCode)
        {
            expirationTimespanInSeconds = 60;
        }
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationTimespanInSeconds)
        };

        var typeKeyCombination = cacheType.ToString() + "_" + cacheKey;
        _memoryCache.Set(typeKeyCombination, cacheValue, cacheEntryOptions);
    }

    public T GetCache<T>(CacheType cacheType, string cacheKey)
    {
        var typeKeyCombination = cacheType.ToString() + "_" + cacheKey;
        Console.WriteLine(typeKeyCombination);
        try
        {
            var cacheData = _memoryCache.Get<T>(typeKeyCombination);
            return cacheData;
        }
        catch
        {
            return default(T);
        }
    }

    public void DeleteCache(CacheType cacheType, string cacheKey)
    {
        var typeKeyCombination = cacheType.ToString() + "_" + cacheKey;
        _memoryCache.Remove(typeKeyCombination);
    }


}
