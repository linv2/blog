using MetroBlog.Core.Common;
using System.Collections.Generic;

namespace MetroBlog.Core.Data.IService
{
    public interface ITagService
    {
        PageInfo<IList<Model.ViewModel.Tag>> SelectTagList(int pageIndex = 1, int pageSize = 10);


        int UpdateTag(Model.ViewModel.Tag mTag);

        Model.ViewModel.Tag SelectTagByName(string tagName);
        Model.ViewModel.Tag SelectTagById(int tagId);
    }
}
