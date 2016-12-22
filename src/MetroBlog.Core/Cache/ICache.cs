using System.Collections.Generic;

namespace MetroBlog.Core.Cache
{

    public interface ICache
    {
        object Get(string key);
        IEnumerable<TSource> Get<TSource>(params string[] key);
        TSource Get<TSource>(string key);
        bool Save<TSource>(string key, TSource value);
        void Remove(string key);
    }
}
