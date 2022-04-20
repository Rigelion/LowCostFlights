using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces;

public interface ICacheService
{
    Task<T> GetFromCacheAndCache<T>(string cacheKey, Func<Task<T>> func);
}

