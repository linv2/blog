using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.Service
{
    public class MenuService: IMenuService
    {

        private const string cacheKey = "menu";
        MenuSqlMap sqlMap;
        ICache cache;
        public MenuService(MenuSqlMap sqlMap, ICache cache)
        {
            this.sqlMap = sqlMap;
            this.cache = cache;
        }
        public Rsp AddMenu(Model.ViewModel.Menu mMenu)
        {
            var checkInfo = mMenu.Check<SaveMenuValidator, Model.ViewModel.Menu>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            mMenu.CreateTime = DateTime.Now;
            var articleId = sqlMap.AddMenu(mMenu);
            if (articleId > 0)
            {
                cache.Delete(cacheKey);
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
            var articleId = sqlMap.UpdateMenu(mMenu);
            if (articleId > 0)
            {
                cache.Delete(cacheKey);
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Model.ViewModel.Menu SelectMenuById(int menuId)
        {
            return sqlMap.SelectMenuById(menuId);
        }
        public IList<Model.ViewModel.Menu> SelectMenuList()
        {
            IList<Model.ViewModel.Menu> list = cache.Get<IList<Model.ViewModel.Menu>>(cacheKey);
            if (list == null)
            {
                list = sqlMap.SelectMenuList();
                cache.Save(cacheKey, list);
            }
            return list;
        }
    }
}
