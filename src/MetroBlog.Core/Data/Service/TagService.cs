using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Data.IBatisNet.SqlMap;
using MetroBlog.Core.Data.IService;
using System.Collections.Generic;

namespace MetroBlog.Core.Data.Service
{
    public class TagService : ITagService
    {
        private readonly TagSqlMap _sqlMap;
        private readonly ICache _cache;
        public TagService(TagSqlMap sqlMap, ICache cache)
        {
            _sqlMap = sqlMap;
            _cache = cache;
        }

        public PageInfo<IList<Model.ViewModel.Tag>> SelectTagList(int pageIndex = 1, int pageSize = 10)
        {
            var count = _sqlMap.SelectTagCount();
            var pageInfo = Utility.MathPage<IList<Model.ViewModel.Tag>>(count, pageIndex, pageSize);
            var query = new TagQuery();
            query.StartRow = (pageInfo.PageIndex - 1) * pageInfo.PageSize + 1;
            query.EndRow = pageInfo.PageIndex * pageInfo.PageSize;
            pageInfo.Data = _sqlMap.SelectTagList(query);
            return pageInfo;
        }

        public int UpdateTag(Model.ViewModel.Tag mTag)
        {
            return _sqlMap.UpdateTag(mTag);
        }

        public Model.ViewModel.Tag SelectTagByName(string tagName)
        {
            return _sqlMap.SelectTagByName(tagName);
        }
        public Model.ViewModel.Tag SelectTagById(int tagId)
        {
            return _sqlMap.SelectTagById(tagId);

        }
    }
}
