using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            foreach (var item in setting)
            {
                sqlMap.SaveSetting(item.Key, item.Value);
            }
            cache.Remove(cacheKey);
            return true;
        }
        public Model.ViewModel.Setting GetSetting()
        {
            Model.ViewModel.Setting result = cache.Get<Model.ViewModel.Setting>(cacheKey);
            if (result == null)
            {
                result = sqlMap.SelectSetting();
                if (result == null)
                {
                    result = new Model.ViewModel.Setting();
                    result.DesKey = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                    result.DesVi = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                }
                cache.Save(cacheKey, result);
            }
            return result;
        }

    }
}
