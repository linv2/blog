using System;
using System.Dynamic;
using System.Web;
using MetroBlog.Core;
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
                if (view != null)
                {
                    context.Response.ContentType = view.ContentType;
                    currentTheme.Template.Render(view, context.Response.OutputStream);
                }
                if (view == null || context.Response.StatusCode == 404)
                {
                    ResponseNotFound(currentTheme, context);
                }

            }
            catch (Exception e)
            {
                if (Blog.Current.Setting.ThrowError)
                {
                    throw e;
                }
                else
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
                    catch (Exception ex)
                    {
                        if (Blog.Current.Setting.ThrowError)
                        {
                            throw ex;
                        }
                        else
                        {
                            context.Response.StatusCode = 503;
                        }
                    }
                }

            }

            GZipCompress.CheckForCompression(context);

        }
        public void ResponseNotFound(ThemesManage themesManage, HttpContext context)
        {
            var view = themesManage.FindView(context.Request.Url, "404");
            if (view == null)
            {
                context.Response.StatusCode = 404;
                return;
            }
            context.Response.ContentType = view.ContentType;
            themesManage.Template.Render(view, context.Response.OutputStream);
        }

    }
}
