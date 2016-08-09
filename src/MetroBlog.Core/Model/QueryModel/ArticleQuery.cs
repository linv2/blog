using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Model.QueryModel
{
    public class ArticleQuery : IDbModelface
    {
        public Int32 StartRow { get; set; }
        public Int32 EndRow { get; set; }
        public Int32 Year { get; set; }
        public Int32 Month { get; set; }
        public Int32 Day { get; set; }
        public int CategoryId { get; set; }

        public int Status { get; set; } = -1;
        public string KeyWord { get; set; }
        public Int32 TagId { get; set; }
        public string TagName { get; set; }

    }
}
