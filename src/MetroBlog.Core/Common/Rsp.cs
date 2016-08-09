using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core.Common
{
    public class Rsp
    {
        public static Rsp Success { get; set; } = new Rsp();
        public bool error { get; set; }
        public string message { get; set; }


        public static Rsp Create(bool error, string message)
        {
            return new Rsp()
            {
                error = error,
                message = message
            };
        }
        public static Rsp Error(string message)
        {
            return new Rsp()
            {
                error = true,
                message = message
            };
        }
        public static Rsp<TSource> Error<TSource>(string message)
        {
            return new Rsp<TSource>()
            {
                error = true,
                message = message
            };
        }
        public static Rsp<TSource> Create<TSource>(TSource data)
        {
            return new Rsp<TSource>()
            {
                data = data
            };
        }
    }
    public class Rsp<TSource> : Rsp
    {
        public TSource data { get; set; }
    }
}
