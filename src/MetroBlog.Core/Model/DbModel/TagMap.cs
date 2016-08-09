using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Model.DbModel
{
    public class TagMap
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ArticleId { get; set; }
    }
}
