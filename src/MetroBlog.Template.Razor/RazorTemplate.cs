using System;
using System.IO;
using MetroBlog.Core;
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
            config.Debug = true;
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
                _razorEngine.Compile(view.FileContent, view.ViewFile);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Render(Views view, Stream sm)
        {

        }


        public void Render(Views view, TextWriter writer)
        {
            try
            {

                _razorEngine.Run(view.ViewFile, writer, typeof(Blog), Blog.Current);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Compile(string name, string templateSource)
        {
            _razorEngine.Compile(templateSource, name);
        }
    }


}
