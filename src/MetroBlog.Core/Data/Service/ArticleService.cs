using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Model.ViewModel;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.Service
{
    public class ArticleService : IArticleService
    {
        private const string cacheKey = "Article_";
        ArticleSqlMap sqlMap;
        ICache cache;
        public ArticleService(ArticleSqlMap sqlMap, ICache cache)
        {
            this.sqlMap = sqlMap;
            this.cache = cache;
        }

        public Rsp AddArticle(Model.ViewModel.Article mArticle)
        {
            var checkInfo = mArticle.Check<SaveArticleValidator, Model.ViewModel.Article>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            mArticle.UId = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(mArticle.Alias))
            {
                mArticle.Alias = mArticle.Alias;
            }
            mArticle.CreateTime = DateTime.Now;
            var articleId = sqlMap.AddArticle(mArticle);
            if (articleId > 0)
            {
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Rsp UpdateArticle(Model.ViewModel.Article mArticle)
        {
            var checkInfo = mArticle.Check<SaveArticleValidator, Model.ViewModel.Article>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = sqlMap.UpdateArticle(mArticle);
            if (articleId > 0)
            {
                cache.Remove(string.Concat(cacheKey, mArticle.Id.ToString()));
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Model.ViewModel.Article SelectArticleById(int articleId)
        {
            Article articleInfo = cache.Get<Article>(string.Concat(cacheKey, articleId.ToString()));
            if (articleInfo == null)
            {
                articleInfo = sqlMap.SelectArticleById(articleId);
                cache.Save(string.Concat(cacheKey, articleId.ToString()), articleInfo);
            }
            return articleInfo;
        }

        public PageInfo<IList<Model.ViewModel.Article>> SelectArticleList(ArticleQuery query, int pageIndex = 1, int pageSize = 10)
        {
            int count = sqlMap.SelectArticleCount(query);
            var pageInfo = Utility.MathPage<IList<Model.ViewModel.Article>>(count, pageIndex, pageSize);
            query.StartRow = (pageInfo.PageIndex - 1) * pageInfo.PageSize + 1;
            query.EndRow = pageInfo.PageIndex * pageInfo.PageSize;
            pageInfo.Data = sqlMap.SelectArticleList(query);
            return pageInfo;
        }
    }
}
