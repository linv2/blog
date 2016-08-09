using MetroBlog.Settings;
using Nancy.Hosting.Aspnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MetroBlog.Template
{
    public class TemplateHandlerFactory : IHttpHandlerFactory
    {
        static bool repeatInit = false;
        static TemplateHttpHandler templateHttpHandler = null;
        static NancyHttpRequestHandler nancyHttpHandler = null;
        public TemplateHandlerFactory()
        {
            if (!repeatInit)
            {
                
                BlogSetting.Register("system", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "t", "system.config"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "system.config"));

                BlogSetting.Register("comment", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "t", "comment.config"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "comment.config"));

                nancyHttpHandler = new NancyHttpRequestHandler();
                templateHttpHandler = new TemplateHttpHandler();
                repeatInit = true;
            }
        }

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            var absolutePath = context.Request.Url.AbsolutePath;
            if (templateHttpHandler.checkHttpContext(context))
            {
                return templateHttpHandler;
            }
            else
            {
                return nancyHttpHandler;
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
