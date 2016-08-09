using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MetroBlog.Core.Model.DbModel
{
    public class Category : IDbModelface
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string KeyWord { get; set; }
        public string Description { get; set; }

        public int SortId { get; set; }
        public bool Delete { get; set; }
        public int Count { get; set; }
    }
}
