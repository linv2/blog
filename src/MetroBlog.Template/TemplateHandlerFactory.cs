using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Nancy.Hosting.Aspnet;

namespace MetroBlog.Template
{
    public class TemplateHandlerFactory : IHttpHandlerFactory
    {
        private static bool _repeatInit = false;
        private static TemplateHttpHandler _templateHttpHandler;
        private static NancyHttpRequestHandler _nancyHttpHandler;
        public TemplateHandlerFactory()
        {
            if (_repeatInit) return;
            _nancyHttpHandler = new NancyHttpRequestHandler();
            _templateHttpHandler = new TemplateHttpHandler();
            _repeatInit = true;
        }

        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string url,
            string pathTranslated)
        {
          
            if (_templateHttpHandler.CheckHttpContext(context))
            {
                return _templateHttpHandler;
            }
            return _nancyHttpHandler;
        }


        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {
            
        }
    }
}
