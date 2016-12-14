using System.Collections.Generic;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class SettingSqlMap : SqlMapBase
    {
        public int SaveSetting(string key, string value)
        {
            return Update("saveSetting", new Model.DbModel.Setting()
            {
                Key = key,
                Value = value
            });
        }
        public IList<Model.DbModel.Setting> SelectSetting()
        {
            return QueryForList<Model.DbModel.Setting>("selectSetting", null);
        }
    }
}
