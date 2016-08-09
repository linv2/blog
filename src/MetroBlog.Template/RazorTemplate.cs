using MetroBlog.Core;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.IO;

namespace MetroBlog.Template
{
    public class RazorTemplate
    {
        IRazorEngineService razorEngine;
        public RazorTemplate()
        {
            TemplateServiceConfiguration config = new TemplateServiceConfiguration();
            config.Debug = true;
            config.Namespaces.Add("MetroBlog.Core");
            config.Namespaces.Add("MetroBlog.Core.Model.ViewModel");
            config.Namespaces.Add("System.Web");
            config.Namespaces.Add("System");
            razorEngine = RazorEngineService.Create(config);
        }
        public void Compile(Views view)
        {
            try
            {
                razorEngine.Compile(view.FileContent, view.ViewFile);
            }
            catch (Exception ex)
            {

            }
        }
        public void Compile(string name, string templateSource)
        {
            razorEngine.Compile(templateSource, name);
        }
        public void Run(Views view, TextWriter writer)
        {
            razorEngine.RunCompile(view.ViewFile, writer, typeof(Blog), Blog.Current);
        }
    }
}
