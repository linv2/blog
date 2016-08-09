using MetroBlog.Core.Data.IBatisNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class CategorySqlMap : SqlMapBase
    {
        public int AddCategory(Model.ViewModel.Category mCategory)
        {
            return base.Insert<int>("addCategory", mCategory);
        }

        public int UpdateCategory(Model.ViewModel.Category mCategory)
        {
            return base.Update("updateCategory", mCategory);
        }

        public Model.ViewModel.Category SelectCategoryById(int categoryId)
        {
            return base.QueryForObject<Model.ViewModel.Category>("selectCategoryById", categoryId);

        }
        public int SelectCategoryArticleCount(int categoryId)
        {
            return base.QueryForObject<int>("selectCategoryArticleCount", categoryId);

        }

        public int UpdateCategoryArticleCount(int categoryId = 0)
        {
            return base.Update("updateCategoryArticleCount", categoryId);
        }

        public IList<Model.ViewModel.Category> SelectCategoryList()
        {
            return base.QueryForList<Model.ViewModel.Category>("selectCategoryList", null);
        }
    }
}
