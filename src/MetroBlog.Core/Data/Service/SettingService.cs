using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using System;
using System.Linq;
using MetroBlog.Core.Model.ViewModel;

namespace MetroBlog.Core.Data.Service
{
    public class SettingService : ISettingService
    {
        private const string CacheKey = "setting";
        readonly SettingSqlMap _sqlMap;
        readonly ICache _cache;
        public SettingService(SettingSqlMap sqlMap, ICache cache)
        {
            _sqlMap = sqlMap;
            _cache = cache;
        }
        public bool SaveSetting(Setting setting)
        {
            var properties = setting.GetType().GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    var value = Convert.ToString(property.GetValue(setting, null));
                    _sqlMap.SaveSetting(property.Name, value);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            _cache.Remove(CacheKey);
            return true;
        }
        public Setting GetSetting()
        {
            var result = _cache.Get<Setting>(CacheKey);
            if (result != null) return result;
            var dbSetting = _sqlMap.SelectSetting();
            if (dbSetting.Count > 0)
            {
                result = new Setting();
                var properties = result.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var settingItem = dbSetting.FirstOrDefault(x => x.Key == property.Name);
                    if (settingItem != null)
                    {
                        try
                        {

                            var value = Convert.ChangeType(settingItem.Value, property.PropertyType);
                            property.SetValue(result, value, null);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            if (result == null)
            {
                result = new Setting();
            }
            _cache.Save(CacheKey, result);
            return result;
        }

    }
}
