using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using GunterBar.Application.Interfaces;

namespace GunterBar.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        try
        {
            if (!_cache.TryGetValue(key, out T cachedValue))
            {
                cachedValue = await factory();

                var cacheOptions = new MemoryCacheEntryOptions();
                if (expiration.HasValue)
                {
                    cacheOptions.SetAbsoluteExpiration(expiration.Value);
                }
                else
                {
                    cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(10));
                }

                _cache.Set(key, cachedValue, cacheOptions);
                _logger.LogInformation("Cache miss for key {Key}", key);
            }
            else
            {
                _logger.LogInformation("Cache hit for key {Key}", key);
            }

            return cachedValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error accessing cache for key {Key}", key);
            throw;
        }
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        _logger.LogInformation("Removed cache entry for key {Key}", key);
    }

    public void Clear()
    {
        if (_cache is MemoryCache memoryCache)
        {
            memoryCache.Compact(1.0);
            _logger.LogInformation("Cache cleared");
        }
    }
}
