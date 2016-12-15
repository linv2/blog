using System;
using System.Web;

namespace MetroBlog.Template
{
    public class TemplateHttpHandler : IHttpHandler
    {
        public TemplateHttpHandler()
        {
            SessionManage.ThemeName = "Default";
        }

        public bool IsReusable { get; } = true;

        private void ResponseView(HttpContext context, Views view)
        {
            context.Response.ContentType = view.ContentType;
            view.Render(context.Response.OutputStream);
        }
        public bool CheckHttpContext(HttpContext context)
        {
            var absolutePath = context.Request.Url.AbsolutePath;
            var currentTheme = SessionManage.CurrentTheme;
            var view = currentTheme.FindView(absolutePath);
            return view != null;
        }
        public void ProcessRequest(HttpContext context)
        {
            var absolutePath = context.Request.Url.AbsolutePath;
            var currentTheme = SessionManage.CurrentTheme;
            var view = currentTheme.FindView(absolutePath);
            if (view == null) return;
            try
            {
                ResponseView(context, view);
            }
            finally
            {
             //   context.Response.End();
            }
        }
    }
}
