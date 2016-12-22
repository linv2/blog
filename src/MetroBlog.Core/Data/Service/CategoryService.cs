using System;
using System.Collections.Generic;
using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Category;
using MetroBlog.Core.Cache;

namespace MetroBlog.Core.Data.Service
{
    public class CategoryService : ICategoryService
    {
        readonly CategorySqlMap _sqlMap;
        public CategoryService(CategorySqlMap sqlMap)
        {
            _sqlMap = sqlMap;
            SelectCategoryList();
        }
        public Rsp AddCategory(Model.ViewModel.Category mCategory)
        {
            var checkInfo = mCategory.Check<SaveCategoryValidator, Model.ViewModel.Category>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            if (string.IsNullOrEmpty(mCategory.Alias))
            {
                mCategory.Alias = Guid.NewGuid().ToString("N");
            }
            var categoryId = _sqlMap.AddCategory(mCategory);
            if (categoryId > 0)
            {
                CacheManage.RemoveCategory();
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Rsp UpdateCategory(Model.ViewModel.Category mCategory)
        {

            var checkInfo = mCategory.Check<SaveCategoryValidator, Model.ViewModel.Category>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            if (string.IsNullOrEmpty(mCategory.Alias))
            {
                mCategory.Alias = Guid.NewGuid().ToString("N");
            }
            var categoryId = _sqlMap.UpdateCategory(mCategory);
            if (categoryId > 0)
            {
                CacheManage.RemoveCategory();
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Model.ViewModel.Category SelectCategoryById(int categoryId)
        {
            var category = CacheManage.GetCategory(categoryId);
            if (category != null) return category;
            category = _sqlMap.SelectCategoryById(categoryId);
            category?.SaveToCache();
            return category;
        }
        public Model.ViewModel.Category SelectCategoryByAlias(string alias)
        {
            var category = CacheManage.GetCategory(alias);
            if (category != null) return category;
            category = _sqlMap.SelectCategoryByAlias(alias);
            category?.SaveToCache();
            return category;
        }
        public int SelectCategoryArticleCount(int categoryId)
        {
            return _sqlMap.SelectCategoryArticleCount(categoryId);

        }

        public int UpdateCategoryArticleCount(int categoryId = 0)
        {
            return _sqlMap.UpdateCategoryArticleCount(categoryId);
        }

        public IList<Model.ViewModel.Category> SelectCategoryList()
        {
            var list = CacheManage.GetCategoryList();
            if (list != null) return list;
            list = _sqlMap.SelectCategoryList();
            foreach (var item in list)
            {
                item.SaveToCache();
            }
            list?.SaveToCache();
            return list;
        }
    }
}
