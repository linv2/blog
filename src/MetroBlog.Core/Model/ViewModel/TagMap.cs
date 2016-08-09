using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Model.ViewModel
{
    public class TagMap : DbModel.Tag
    {
        public int ArticleId { get; set; }
    }
}
