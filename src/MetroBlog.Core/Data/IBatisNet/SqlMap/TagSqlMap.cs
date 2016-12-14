using MetroBlog.Core.Model.QueryModel;
using System;
using System.Collections.Generic;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class TagSqlMap : SqlMapBase
    {
        public int AddTag(string tagName)
        {
            var mTag = new Model.ViewModel.Tag();
            mTag.TagName = tagName;
            mTag.Count = 0;
            mTag.CreateTime = DateTime.Now;
            mTag.Delete = false;
            return Insert<int>("addTag", mTag);
        }

        public int UpdateTag(Model.ViewModel.Tag mTag)
        {
            return Update("updateTag", mTag);
        }

        public Model.ViewModel.Tag SelectTagByName(string tagName)
        {
            return QueryForObject<Model.ViewModel.Tag>("selectTagByName", tagName);
        }
        public Model.ViewModel.Tag SelectTagById(int tagId)
        {
            return QueryForObject<Model.ViewModel.Tag>("selectTagById", tagId);

        }

        public int DeleteTagMapByArticleId(int articleId)
        {
            return Delete("deleteTagMapByArticleId", articleId);
        }
        internal int AddTagMap(int tagId, int articleId)
        {
            var mTag = new Model.ViewModel.TagMap();
            mTag.Id = tagId;
            mTag.ArticleId = articleId;
            return Insert<int>("addTagMap", mTag);
        }

        public int SelectTagCount()
        {
            return QueryForObject<int>("selectTagCount", null);
        }
        public IList<Model.ViewModel.Tag> SelectTagList(TagQuery query)
        {
            return QueryForList<Model.ViewModel.Tag>("selectTagList", query);
        }
    }
}
