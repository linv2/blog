using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Template.Razor
{
    public class WebTemplate<T> : TemplateBase<T>
    {
        public System.Web.HttpRequest Request
        {
            get
            {
                return System.Web.HttpContext.Current.Request;
            }

        }
        public MetroBlog.Core.Blog Blog
        {
            get
            {
                return MetroBlog.Core.Blog.Current;
            }
        }
    }
}
