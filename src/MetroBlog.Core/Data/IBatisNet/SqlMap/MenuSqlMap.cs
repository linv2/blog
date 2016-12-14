using System.Collections.Generic;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class MenuSqlMap : SqlMapBase
    {
        public int AddMenu(Model.ViewModel.Menu mMenu)
        {
            return Insert<int>("addMenu", mMenu);
        }
        public int UpdateMenu(Model.ViewModel.Menu mMenu)
        {
            return Update("updateMenu", mMenu);
        }

        public Model.ViewModel.Menu SelectMenuById(int menuId)
        {
            return QueryForObject<Model.ViewModel.Menu>("selectMenuById", menuId);
        }
        public IList<Model.ViewModel.Menu> SelectMenuList()
        {
            return QueryForList<Model.ViewModel.Menu>("selectMenuList", null);
        }
    }
}
