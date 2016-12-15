using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Menu;
using System;
using System.Collections.Generic;

namespace MetroBlog.Core.Data.Service
{
    public class MenuService : IMenuService
    {

        private const string CacheKey = "menu";
        readonly MenuSqlMap _sqlMap;
        readonly ICache _cache;
        public MenuService(MenuSqlMap sqlMap, ICache cache)
        {
            _sqlMap = sqlMap;
            _cache = cache;
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
                _cache.Remove(CacheKey);
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
                _cache.Remove(CacheKey);
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
            var list = _cache.Get<IList<Model.ViewModel.Menu>>(CacheKey);
            if (list == null)
            {
                list = _sqlMap.SelectMenuList();
                _cache.Save(CacheKey, list);
            }
            return list;
        }
    }
}
