using System;
namespace MetroBlog.Core.Model.QueryModel
{
    public class ArticleQuery : IDbModelface
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int CategoryId { get; set; }

        public int Status { get; set; } = -1;
        public string KeyWord { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }

    }
}
