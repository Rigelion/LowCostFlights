using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetFromCacheAndCache<T>(string cacheKey, Func<Task<T>> func)
        {
            if (_memoryCache.TryGetValue(cacheKey, out T result))
            {
                return result;
            }

            result = await func();

            _memoryCache.Set(cacheKey, result);

            return result;

        }
    }
}
