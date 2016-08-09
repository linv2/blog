using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class CategoryService : ICategoryService
    {
        private const string cacheKey = "Category";
        CategorySqlMap sqlMap;
        ICache cache;
        public CategoryService(CategorySqlMap sqlMap, ICache cache)
        {
            this.sqlMap = sqlMap;
            this.cache = cache;
        }
        public Rsp AddCategory(Model.ViewModel.Category mCategory)
        {
            var checkInfo = mCategory.Check<SaveCategoryValidator, Model.ViewModel.Category>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = sqlMap.AddCategory(mCategory);
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

        public Rsp UpdateCategory(Model.ViewModel.Category mCategory)
        {

            var checkInfo = mCategory.Check<SaveCategoryValidator, Model.ViewModel.Category>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = sqlMap.UpdateCategory(mCategory);
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

        public Model.ViewModel.Category SelectCategoryById(int categoryId)
        {
            return sqlMap.SelectCategoryById(categoryId);

        }
        public int SelectCategoryArticleCount(int categoryId)
        {
            return sqlMap.SelectCategoryArticleCount(categoryId);

        }

        public int UpdateCategoryArticleCount(int categoryId = 0)
        {
            return sqlMap.UpdateCategoryArticleCount(categoryId);
        }

        public IList<Model.ViewModel.Category> SelectCategoryList()
        {
            IList<Model.ViewModel.Category> list = cache.Get<IList<Model.ViewModel.Category>>(cacheKey);
            if (list == null)
            {
                list = sqlMap.SelectCategoryList();
                cache.Save(cacheKey, list);
            }
            return list;
        }
    }
}
