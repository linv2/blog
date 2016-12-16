using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Windows.Forms;
using System.Xml;
using MetroBlog.Template.View;

namespace MetroBlog.Template.View
{
    /// <summary>
    /// 页面
    /// </summary>
    public class PageView : Views
    {

        public override bool Match(Uri requestUri, string requestIndex = null)
        {
            return requestUri.AbsolutePath == RequestIndex;
        }
    }
}
