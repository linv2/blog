using System;
using System.Dynamic;
using System.Web;
using MetroBlog.Template.View;

namespace MetroBlog.Template
{
    public class TemplateHttpHandler : IHttpHandler
    {
        public TemplateHttpHandler()
        {
            SessionManage.ThemeName = "Default";
        }

        public bool IsReusable { get; } = true;


        public bool CheckHttpContext(HttpContext context)
        {
            var currentTheme = SessionManage.CurrentTheme;
            var view = currentTheme.FindView(context.Request.Url);
            return view != null;
        }
        public void ProcessRequest(HttpContext context)
        {
            var currentTheme = SessionManage.CurrentTheme;
            try
            {
                var view = currentTheme.FindView(context.Request.Url);
                if (view == null)
                {
                    view = currentTheme.FindView(context.Request.Url, "404");
                    if (view == null)
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }
                };

                context.Response.ContentType = view.ContentType;
                currentTheme.Template.Render(view, context.Response.OutputStream);
            }
            catch (Exception e)
            {
                try
                {

                    var view = currentTheme.FindView(context.Request.Url, "Error");
                    if (view == null)
                    {
                        context.Response.StatusCode = 503;
                        return;
                    }
                    dynamic dynamicObject = new ExpandoObject();
                    dynamicObject.Error = e;

                    currentTheme.Template.Render(view, context.Response.OutputStream, dynamicObject);
                }
                catch (Exception)
                {
                    context.Response.Write("Error");
                }

            }
            finally
            {
                //   context.Response.End();
            }
        }


    }
}
