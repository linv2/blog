using MetroBlog.Core.Data.IBatisNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class MenuSqlMap : SqlMapBase
    {
        public int AddMenu(Model.ViewModel.Menu mMenu)
        {
            return base.Insert<int>("addMenu", mMenu);
        }
        public int UpdateMenu(Model.ViewModel.Menu mMenu)
        {
            return base.Update("updateMenu", mMenu);
        }

        public Model.ViewModel.Menu SelectMenuById(int menuId)
        {
            return base.QueryForObject<Model.ViewModel.Menu>("selectMenuById", menuId);
        }
        public IList<Model.ViewModel.Menu> SelectMenuList()
        {
            return base.QueryForList<Model.ViewModel.Menu>("selectMenuList", null);
        }
    }
}
