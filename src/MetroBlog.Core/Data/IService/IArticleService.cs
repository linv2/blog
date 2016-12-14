using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Model.ViewModel;
using System.Collections.Generic;

namespace MetroBlog.Core.Data.IService
{
    public interface IArticleService
    {
        Rsp AddArticle(Article mArticle);

        Rsp UpdateArticle(Article mArticle);

        Article SelectArticleById(int articleId);

        PageInfo<IList<Article>> SelectArticleList(ArticleQuery query, int pageIndex = 1, int pageSize = 10);
    }
}
