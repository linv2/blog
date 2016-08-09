using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace MetroBlog.Core.Providers
{
    public class CacheProvider : ICache
    {
        MemoryCache cache = MemoryCache.Default;
        public void Delete(string key)
        {
            cache.Remove(key);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public TSource Get<TSource>(string key)
        {
            return (TSource)cache.Get(key);
        }

        public bool Save<TSource>(string key, TSource value)
        {
            return cache.Add(key, value, DateTimeOffset.MaxValue);
        }
    }
}
