using MetroBlog.Core.Data.IBatisNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class ArticleSqlMap : SqlMapBase
    {
        TagSqlMap tagSqlMap = new TagSqlMap();


        private void UpdateArticleTag(List<Model.ViewModel.Tag> list, int articleId)
        {
            if (list != null)
            {
                tagSqlMap.DeleteTagMapByArticleId(articleId);
                int tagId = 0;
                foreach (var tagItem in list)
                {
                    var tagName = tagItem.TagName;
                    var tagInfo = tagSqlMap.SelectTagByName(tagName);
                    if (tagInfo == null)
                    {
                        tagId = tagSqlMap.AddTag(tagName);
                    }
                    else
                    {
                        tagId = tagInfo.Id;
                    }
                    tagSqlMap.AddTagMap(tagId, articleId);
                }

            }
        }
        public int AddArticle(Model.ViewModel.Article mArticle)
        {
            using (var tran = sqlMapper.BeginTransaction())
            {
                try
                {
                    var articleId = base.Insert<int>("addArticle", mArticle);
                    UpdateArticleTag(mArticle.Tags, articleId);
                }
                catch
                {
                    tran.RollBackTransaction();
                }
            }

            return base.Insert<int>("addArticle", mArticle);
        }

        public int UpdateArticle(Model.ViewModel.Article mArticle)
        {
            UpdateArticleTag(mArticle.Tags, mArticle.Id);
            return base.Update("updateArticle", mArticle);
        }

        public Model.ViewModel.Article SelectArticleById(int articleId)
        {
            return base.QueryForObject<Model.ViewModel.Article>("selectArticleById", articleId);

        }
        public int SelectArticleCount(Core.Model.QueryModel.ArticleQuery mmArticle)
        {
            return base.QueryForObject<Int32>("selectArticleCount", mmArticle);
        }
        public IList<Model.ViewModel.Article> SelectArticleList(Core.Model.QueryModel.ArticleQuery mmArticle)
        {
            return base.QueryForList<Model.ViewModel.Article>("selectArticleList", mmArticle);
        }

    }
}
