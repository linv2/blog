using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using MetroBlog.Core;
using MetroBlog.Template.View;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace MetroBlog.Template.Razor
{
    public class RazorTemplate : ITemplate
    {
        readonly IRazorEngineService _razorEngine;

        public RazorTemplate()
        {
            var config = new TemplateServiceConfiguration();
           // config.Debug = true;
            config.Namespaces.Add("MetroBlog.Core");
            config.Namespaces.Add("MetroBlog.Core.Model.ViewModel");
            config.Namespaces.Add("System.Web");
            config.Namespaces.Add("System");
            config.BaseTemplateType = typeof(WebTemplate);
            _razorEngine = RazorEngineService.Create(config);

        }


        public void Compile(Views view)
        {
            try
            {
                _razorEngine.Compile(view.FileContent, view.RequestIndex);

            }
            catch (Exception ex)
            {
                if (Blog.Current.Setting.ThrowError)
                    throw ex;
            }
        }
        public void Compile(string name, string templateSource)
        {
            var key = _razorEngine.GetKey(name, ResolveType.Layout);
            _razorEngine.AddTemplate(key, templateSource);
        }


        public void Render(Views view, Stream sm, dynamic value = null)
        {
            try
            {
                Type valueType = null;
                if (value != null)
                {
                    valueType = value.GetType();
                }
                using (var writer = new StreamWriter(sm))
                {
                    _razorEngine.RunCompile(view.RequestIndex, writer);
                }
            }
            catch (Exception ex)
            {
                if (Blog.Current.Setting.ThrowError)
                    throw new Exception("编译异常:" + view.FileName, ex);
            }
        }


        public void Render(Views view, TextWriter writer, dynamic value = null)
        {
            try
            {
                _razorEngine.RunCompile(view.RequestIndex, writer);
            }
            catch (Exception ex)
            {
                if (Blog.Current.Setting.ThrowError)
                    throw new Exception("编译异常:" + view.FileName, ex);
            }
        }

    }


}
