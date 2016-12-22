using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Model.ViewModel;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Validator.Article;
using System;
using System.Collections.Generic;
using MetroBlog.Core.Cache;

namespace MetroBlog.Core.Data.Service
{
    public class ArticleService : IArticleService
    {
        private const string CacheKey = "Article_";
        readonly ArticleSqlMap _sqlMap;
        readonly ICache _cache;
        public ArticleService(ArticleSqlMap sqlMap, ICache cache)
        {
            _sqlMap = sqlMap;
            _cache = cache;
        }

        public Rsp AddArticle(Article mArticle)
        {
            var checkInfo = mArticle.Check<SaveArticleValidator, Article>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            mArticle.UId = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(mArticle.Alias))
            {
                mArticle.Alias = mArticle.Title;
            }
            mArticle.CreateTime = DateTime.Now;
            var articleId = _sqlMap.AddArticle(mArticle);
            return articleId > 0 ? Rsp.Success : Rsp.Error("数据保存失败");
        }

        public Rsp UpdateArticle(Article mArticle)
        {
            var checkInfo = mArticle.Check<SaveArticleValidator, Article>();
            if (checkInfo.error)
            {
                return checkInfo;
            }
            var articleId = _sqlMap.UpdateArticle(mArticle);
            if (articleId > 0)
            {
                _cache.Remove(string.Concat(CacheKey, mArticle.Id.ToString()));
                return Rsp.Success;
            }
            else
            {
                return Rsp.Error("数据保存失败");
            }
        }

        public Article SelectArticleById(int articleId)
        {
            var articleInfo = CacheManage.GetArticle(articleId);
            if (articleInfo != null) return articleInfo;
            articleInfo = _sqlMap.SelectArticleById(articleId);
            articleInfo?.SaveToCache();
            return articleInfo;
        }

        public PageInfo<IList<Article>> SelectArticleList(ArticleQuery query, int pageIndex = 1, int pageSize = 10)
        {
            var count = _sqlMap.SelectArticleCount(query);
            var pageInfo = Utility.MathPage<IList<Article>>(count, pageIndex, pageSize);
            query.StartRow = (pageInfo.PageIndex - 1) * pageInfo.PageSize + 1;
            query.EndRow = pageInfo.PageIndex * pageInfo.PageSize;
            pageInfo.Data = _sqlMap.SelectArticleList(query);
            return pageInfo;
        }
    }
}
