using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using MetroBlog.Core.Cache;

namespace MetroBlog.Core.Providers
{
    public class CacheProvider : ICache
    {
        readonly MemoryCache _cache = MemoryCache.Default;
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public TSource Get<TSource>(string key)
        {
            return (TSource)_cache.Get(key);
        }

        public bool Save<TSource>(string key, TSource value)
        {
            return _cache.Add(key, value, DateTimeOffset.MaxValue);
        }

        public IEnumerable<TSource> Get<TSource>(params string[] key)
        {
            return null;
        }
    }
}
