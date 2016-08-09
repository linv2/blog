using Nancy;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MetroBlog.Admin
{
    public class NancyDynamicDictionary
    {
        public static NameValueCollection ToNameValueCollection(IDictionary<string,object> dictParam)
        {
            var nvc = new NameValueCollection();
            foreach (var item in dictParam)
            {
                nvc.Add(item.Key, Convert.ToString(item.Value));
            }
            return nvc;
        }
    }
}
