using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Model.DbModel
{
    public class Menu : IDbModelface
    {
        public int Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 打开方式
        /// </summary>
        public string Target { get; set; }

        public int CategoryId { get; set; }

        public int SortId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Hidden { get; set; }
    }
}
