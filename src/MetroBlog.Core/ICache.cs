namespace MetroBlog.Core
{
    public interface ICache
    {
        object Get(string key);
        TSource Get<TSource>(string key);
        bool Save<TSource>(string key, TSource value);
        void Remove(string key);
    }
}
