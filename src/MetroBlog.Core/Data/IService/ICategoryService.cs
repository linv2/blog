using MetroBlog.Core.Common;
using System.Collections.Generic;

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
