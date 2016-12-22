using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using System;
using System.Linq;
using MetroBlog.Core.Cache;
using MetroBlog.Core.Model.ViewModel;

namespace MetroBlog.Core.Data.Service
{
    public class SettingService : ISettingService
    {
        readonly SettingSqlMap _sqlMap;
        public SettingService(SettingSqlMap sqlMap)
        {
            _sqlMap = sqlMap;
        }
        public bool SaveSetting(Setting setting)
        {
            var properties = setting.GetType().GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    if (property.CanWrite)
                    {
                        var value = Convert.ToString(property.GetValue(setting, null));
                        _sqlMap.SaveSetting(property.Name, value);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            CacheManage.RemoveSetting();
            return true;
        }
        public Setting GetSetting()
        {
            var result = CacheManage.GetSetting();
            if (result != null) return result;
            var dbSetting = _sqlMap.SelectSetting();
            if (dbSetting.Count > 0)
            {
                result = new Setting();
                var properties = result.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (!property.CanWrite)
                    {
                        continue;
                    }
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
            result?.SaveToCache();
            return result;
        }

    }
}
