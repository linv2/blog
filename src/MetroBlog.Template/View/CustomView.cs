using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroBlog.Template.View
{
    public class CustomView : Views
    {
        public CustomView()
        {
            base.Level = 10000;
        }

        public override bool Match(Uri requestUri, string requestIndex = null)
        {
            return this.RequestIndex == requestIndex;
        }
    }
}
