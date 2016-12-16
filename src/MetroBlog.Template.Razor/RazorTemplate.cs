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
                _razorEngine.Compile(view.FileContent, view.RequestIndex);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Compile(string name, string templateSource)
        {
            _razorEngine.Compile(templateSource, name);
        }


        public void Render(Views view, Stream sm, dynamic value = null)
        {
            try
            {
                DynamicViewBag dynamicView = null;
                if (value != null)
                {
                    dynamicView = new DynamicViewBag((IDictionary<string, object>)value);
                }
                using (var writer = new StreamWriter(sm))
                {
                    _razorEngine.RunCompile(view.RequestIndex, writer, Page.Current.GetType(), Page.Current, dynamicView);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public void Render(Views view, TextWriter writer, dynamic value = null)
        {
            try
            {
                DynamicViewBag dynamicView = null;
                if (value != null)
                {
                    dynamicView = new DynamicViewBag((IDictionary<string, object>)value);
                }
                _razorEngine.RunCompile(view.RequestIndex, writer, typeof(Page), Page.Current, dynamicView);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }


}
