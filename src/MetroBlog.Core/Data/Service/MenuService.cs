using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Menu;
using System;
using System.Collections.Generic;
using MetroBlog.Core.Cache;

namespace MetroBlog.Core.Data.Service
{
    public class MenuService : IMenuService
    {

        readonly MenuSqlMap _sqlMap;
        public MenuService(MenuSqlMap sqlMap, ICache cache)
        {
            _sqlMap = sqlMap;
        }
        public Rsp AddMenu(Model.ViewModel.Menu mMenu)
        {
            var checkInfo = mMenu.Check<SaveMenuValidator, Model.ViewModel.Menu>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            mMenu.CreateTime = DateTime.Now;
            var articleId = _sqlMap.AddMenu(mMenu);
            if (articleId > 0)
            {
                CacheManage.RemoveMenu();
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }
        public Rsp UpdateMenu(Model.ViewModel.Menu mMenu)
        {
            var checkInfo = mMenu.Check<SaveMenuValidator, Model.ViewModel.Menu>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = _sqlMap.UpdateMenu(mMenu);
            if (articleId > 0)
            {
                CacheManage.RemoveMenu();
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Model.ViewModel.Menu SelectMenuById(int menuId)
        {
            return _sqlMap.SelectMenuById(menuId);
        }
        public IList<Model.ViewModel.Menu> SelectMenuList()
        {
            var list = CacheManage.GetMenu();
            if (list != null) return list;
            list = _sqlMap.SelectMenuList();
            list?.SaveToCache();
            return list;
        }
    }
}
