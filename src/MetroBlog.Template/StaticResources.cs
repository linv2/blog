using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroBlog.Template
{
    public class StaticResources
    {
        public static string[] Folders
        {
            get; set;
        } = { "/public", "/themes" };
    }
}
