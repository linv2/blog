using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Template
{
    public class TempleteDataMemey : Dictionary<string, object>
    {
        public void Put(String key, Object value)
        {
            if (base.ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                base.Add(key, value);
            }
        }
    }
}
