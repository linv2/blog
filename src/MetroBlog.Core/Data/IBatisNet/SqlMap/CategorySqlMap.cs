using System.Collections.Generic;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class CategorySqlMap : SqlMapBase
    {
        public int AddCategory(Model.ViewModel.Category mCategory)
        {
            return Insert<int>("addCategory", mCategory);
        }

        public int UpdateCategory(Model.ViewModel.Category mCategory)
        {
            return Update("updateCategory", mCategory);
        }

        public Model.ViewModel.Category SelectCategoryById(int categoryId)
        {
            return QueryForObject<Model.ViewModel.Category>("selectCategoryById", categoryId);

        }
        public int SelectCategoryArticleCount(int categoryId)
        {
            return QueryForObject<int>("selectCategoryArticleCount", categoryId);

        }

        public int UpdateCategoryArticleCount(int categoryId = 0)
        {
            return Update("updateCategoryArticleCount", categoryId);
        }

        public IList<Model.ViewModel.Category> SelectCategoryList()
        {
            return QueryForList<Model.ViewModel.Category>("selectCategoryList", null);
        }
    }
}
