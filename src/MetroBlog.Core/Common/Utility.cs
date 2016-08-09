using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Web.Security;

namespace MetroBlog.Core.Common
{
    public class Utility
    { /// <summary>
      /// JSON序列化
      /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserializer<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static String Sha1(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "SHA1");
        }

        public static PageInfo<TSource> MathPage<TSource>(int row, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            int pageCount = row / pageSize;
            if (row % pageSize > 0)
            {
                pageCount++;
            }
            return new PageInfo<TSource>()
            {
                Rows = row,
                PageCount = pageCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
    public class PageInfo<TSource>
    {
        public int Rows { get; set; }
        public int PageCount { get; set; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }

        public TSource Data { get; set; }
    }
}
