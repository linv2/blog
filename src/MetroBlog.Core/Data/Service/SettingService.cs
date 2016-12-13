using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroBlog.Core.Model.ViewModel;

namespace MetroBlog.Core.Data.Service
{
    public class SettingService : ISettingService
    {
        private const string cacheKey = "setting";
        SettingSqlMap sqlMap = null;
        ICache cache = null;
        public SettingService(SettingSqlMap sqlMap, ICache cache)
        {
            this.sqlMap = sqlMap;
            this.cache = cache;
        }
        public bool SaveSetting(Model.ViewModel.Setting setting)
        {
            var properties = setting.GetType().GetProperties();
            foreach (var property in properties)
            {
                try
                {


                    var value = Convert.ToString(property.GetValue(setting, null));
                    sqlMap.SaveSetting(property.Name, value);
                }
                catch (Exception)
                {
                }
            }
            cache.Remove(cacheKey);
            return true;
        }
        public Model.ViewModel.Setting GetSetting()
        {
            var result = cache.Get<Model.ViewModel.Setting>(cacheKey);
            if (result != null) return result;
            var dbSetting = sqlMap.SelectSetting();
            if (dbSetting.Count > 0)
            {
                result = new Model.ViewModel.Setting();
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

                        }
                    }
                }
            }
            if (result == null)
            {
                result = new Setting();
            }
            cache.Save(cacheKey, result);
            return result;
        }

    }
}
