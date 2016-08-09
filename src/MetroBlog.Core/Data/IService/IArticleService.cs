using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IService
{
    public interface IArticleService
    {
        Rsp AddArticle(Model.ViewModel.Article mArticle);

        Rsp UpdateArticle(Model.ViewModel.Article mArticle);

        Model.ViewModel.Article SelectArticleById(int articleId);

        PageInfo<IList<Model.ViewModel.Article>> SelectArticleList(ArticleQuery query, int pageIndex = 1, int pageSize = 10);
    }
}
