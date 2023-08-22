namespace AuthService.Services.Abstractions;

public interface IMemoryCachingHandler
{
    public void SetCache<T>(CacheType cacheType, T cacheValue, string cacheKey);
    public T GetCache<T>(CacheType cacheType, string cacheKey);
    public void DeleteCache(CacheType cacheType, string cacheKey);
}
