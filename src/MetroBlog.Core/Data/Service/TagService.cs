using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Data.Service
{
    public class TagService : ITagService
    {
        TagSqlMap sqlMap = new TagSqlMap();

        public PageInfo<IList<Model.ViewModel.Tag>> SelectTagList(int pageIndex = 1, int pageSize = 10)
        {
            int count = sqlMap.SelectTagCount();
            var pageInfo = Utility.MathPage<IList<Model.ViewModel.Tag>>(count, pageIndex, pageSize);
            TagQuery query = new TagQuery();
            query.StartRow = (pageInfo.PageIndex - 1) * pageInfo.PageSize + 1;
            query.EndRow = pageInfo.PageIndex * pageInfo.PageSize;
            pageInfo.Data = sqlMap.SelectTagList(query);
            return pageInfo;
        }

        public int UpdateTag(Model.ViewModel.Tag mTag)
        {
            return sqlMap.UpdateTag(mTag);
        }

        public Model.ViewModel.Tag SelectTagByName(string tagName)
        {
            return sqlMap.SelectTagByName(tagName);
        }
        public Model.ViewModel.Tag SelectTagById(int tagId)
        {
            return sqlMap.SelectTagById(tagId);

        }
    }
}
