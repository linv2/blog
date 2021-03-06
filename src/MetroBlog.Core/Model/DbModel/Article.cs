﻿using System;

namespace MetroBlog.Core.Model.DbModel
{
    public class Article : IDbModelface
    {
        public int Id { get; set; }
        public string UId { get; set; }
        public string Alias { get; set; }
        public string Title { get; set; }

        public string KeyWord { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }

        public bool Comment { get; set; }

        public int CategoryId { get; set; }

        public int Status { get; set; } = -1;

        public int SortId { get; set; }

    }
}
