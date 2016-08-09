using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IBatisNet;
using MetroBlog.Core.Validator.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IService
{
    public interface ICategoryService
    {
        Rsp AddCategory(Model.ViewModel.Category mCategory);

        Rsp UpdateCategory(Model.ViewModel.Category mCategory);

        Model.ViewModel.Category SelectCategoryById(int categoryId);
        int SelectCategoryArticleCount(int categoryId);

        int UpdateCategoryArticleCount(int categoryId = 0);

        IList<Model.ViewModel.Category> SelectCategoryList();
    }
}
