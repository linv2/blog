using System.Collections.Generic;
using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Category;

namespace MetroBlog.Core.Data.Service
{
    public class CategoryService : ICategoryService
    {
        private const string CacheKey = "Category";
        readonly CategorySqlMap _sqlMap;
        readonly ICache _cache;
        public CategoryService(CategorySqlMap sqlMap, ICache cache)
        {
            _sqlMap = sqlMap;
            _cache = cache;
        }
        public Rsp AddCategory(Model.ViewModel.Category mCategory)
        {
            var checkInfo = mCategory.Check<SaveCategoryValidator, Model.ViewModel.Category>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = _sqlMap.AddCategory(mCategory);
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

        public Rsp UpdateCategory(Model.ViewModel.Category mCategory)
        {

            var checkInfo = mCategory.Check<SaveCategoryValidator, Model.ViewModel.Category>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = _sqlMap.UpdateCategory(mCategory);
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

        public Model.ViewModel.Category SelectCategoryById(int categoryId)
        {
            return _sqlMap.SelectCategoryById(categoryId);

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
            var list = _cache.Get<IList<Model.ViewModel.Category>>(CacheKey);
            if (list == null)
            {
                list = _sqlMap.SelectCategoryList();
                _cache.Save(CacheKey, list);
            }
            return list;
        }
    }
}
