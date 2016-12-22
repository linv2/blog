using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Template
{
    public class Page
    {
        public Page()
        {
            Blog = Core.Blog.Current;
        }


        public System.Web.HttpRequest Request
        {
            get { return System.Web.HttpContext.Current.Request; }
        }

        public System.Web.HttpResponse Response
        {

            get { return System.Web.HttpContext.Current.Response; }
        }

        public Core.Blog Blog { get; }

        public string UrlBuild(string url)
        {
            return string.Concat("/themes/", SessionManage.ThemeName, "/", url);
        }

        public string Title => Blog.Setting.Title;

        public static Page Current { get; } = new Page();
    }
}
