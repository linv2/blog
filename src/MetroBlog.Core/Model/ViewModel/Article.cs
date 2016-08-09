using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Model.ViewModel
{
    public class Article : DbModel.Article
    {
        public Article() {
            Tags = new List<Tag>();
        }
        public SimpleArticle Next { get; set; }
        public SimpleArticle Prev { get; set; }
        public Category Category { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
