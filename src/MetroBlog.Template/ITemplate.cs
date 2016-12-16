using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using MetroBlog.Template.View;

namespace MetroBlog.Template
{
    public interface ITemplate
    {
        void Compile(string key, string content);
        void Compile(Views view);

        void Render(Views view, Stream sm, dynamic value = null);

        void Render(Views view, TextWriter writer, dynamic value = null);
    }
}
