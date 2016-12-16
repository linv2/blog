using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroBlog.Template.View
{
    public class RegexView : Views
    {
        public override bool Match(Uri requestUri, string requestIndex = null)
        {
            return Regex.IsMatch(requestUri.PathAndQuery, RequestIndex, RegexOptions.IgnoreCase);
        }
    }
}
