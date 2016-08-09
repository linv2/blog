using MetroBlog.Core.Model.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class SettingSqlMap : SqlMapBase
    {
        public int SaveSetting(string key, string value)
        {
            return base.Update("saveSetting", new Model.DbModel.Setting()
            {
                Key = key,
                Value = value
            });
        }
        public Model.ViewModel.Setting SelectSetting()
        {
            return base.QueryForObject<Model.ViewModel.Setting>("selectSetting", null);
        }
    }
}
