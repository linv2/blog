using MetroBlog.Core.Model.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.IBatisNet.SqlMap
{
    public class TagSqlMap : SqlMapBase
    {
        public int AddTag(string tagName)
        {
            Model.ViewModel.Tag mTag = new Model.ViewModel.Tag();
            mTag.TagName = tagName;
            mTag.Count = 0;
            mTag.CreateTime = DateTime.Now;
            mTag.Delete = false;
            return base.Insert<int>("addTag", mTag);
        }

        public int UpdateTag(Model.ViewModel.Tag mTag)
        {
            return base.Update("updateTag", mTag);
        }

        public Model.ViewModel.Tag SelectTagByName(string tagName)
        {
            return base.QueryForObject<Model.ViewModel.Tag>("selectTagByName", tagName);
        }
        public Model.ViewModel.Tag SelectTagById(int tagId)
        {
            return base.QueryForObject<Model.ViewModel.Tag>("selectTagById", tagId);

        }

        public int DeleteTagMapByArticleId(int articleId)
        {
            return base.Delete("deleteTagMapByArticleId", articleId);
        }
        internal int AddTagMap(int tagId, int articleId)
        {
            Model.ViewModel.TagMap mTag = new Model.ViewModel.TagMap();
            mTag.Id = tagId;
            mTag.ArticleId = articleId;
            return base.Insert<int>("addTagMap", mTag);
        }

        public int SelectTagCount()
        {
            return base.QueryForObject<int>("selectTagCount", null);
        }
        public IList<Model.ViewModel.Tag> SelectTagList(TagQuery query)
        {
            return base.QueryForList<Model.ViewModel.Tag>("selectTagList", query);
        }
    }
}
