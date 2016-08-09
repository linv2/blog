using MetroBlog.Settings;
using Nancy.Hosting.Aspnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MetroBlog.Template
{
    public class TemplateHttpHandler : IHttpHandler
    {
        public TemplateHttpHandler()
        {
            SessionManage.ThemeName = "Default";
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        private void ResponseView(HttpContext context, Views view)
        {
            context.Response.ContentType = "text/html";
            view.Render(context.Response.OutputStream);
        }
        public bool checkHttpContext(HttpContext context)
        {
            var absolutePath = context.Request.Url.AbsolutePath;
            ThemesManage currentTheme = SessionManage.CurrentTheme;
            var view = currentTheme.FindView(absolutePath);
            return view != null;
        }
        public void ProcessRequest(HttpContext context)
        {
            var absolutePath = context.Request.Url.AbsolutePath;
            ThemesManage currentTheme = SessionManage.CurrentTheme;
            var view = currentTheme.FindView(absolutePath);
            if (view != null)
            {

                ResponseView(context, view);
                try
                {
                    context.Response.End();
                }
                catch { }
            }
        }
    }
}
