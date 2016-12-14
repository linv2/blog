using System.Collections.Generic;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class ArticleSqlMap : SqlMapBase
    {
        readonly TagSqlMap _tagSqlMap = new TagSqlMap();


        private void UpdateArticleTag(List<Model.ViewModel.Tag> list, int articleId)
        {
            if (list == null) return;
            _tagSqlMap.DeleteTagMapByArticleId(articleId);
            foreach (var tagItem in list)
            {
                var tagName = tagItem.TagName;
                var tagInfo = _tagSqlMap.SelectTagByName(tagName);
                var tagId = tagInfo?.Id ?? _tagSqlMap.AddTag(tagName);
                _tagSqlMap.AddTagMap(tagId, articleId);
            }
        }
        public int AddArticle(Model.ViewModel.Article mArticle)
        {
            using (var tran = SqlMapper.BeginTransaction())
            {
                try
                {
                    var articleId = Insert<int>("addArticle", mArticle);
                    UpdateArticleTag(mArticle.Tags, articleId);
                }
                catch
                {
                    tran.RollBackTransaction();
                }
            }

            return Insert<int>("addArticle", mArticle);
        }

        public int UpdateArticle(Model.ViewModel.Article mArticle)
        {
            UpdateArticleTag(mArticle.Tags, mArticle.Id);
            return Update("updateArticle", mArticle);
        }

        public Model.ViewModel.Article SelectArticleById(int articleId)
        {
            return QueryForObject<Model.ViewModel.Article>("selectArticleById", articleId);

        }
        public int SelectArticleCount(Model.QueryModel.ArticleQuery mmArticle)
        {
            return QueryForObject<int>("selectArticleCount", mmArticle);
        }
        public IList<Model.ViewModel.Article> SelectArticleList(Model.QueryModel.ArticleQuery mmArticle)
        {
            return QueryForList<Model.ViewModel.Article>("selectArticleList", mmArticle);
        }

    }
}
