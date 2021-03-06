﻿using System;

namespace MetroBlog.Core.Model.DbModel
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public int Count { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Delete { get; set; }
    }
}
