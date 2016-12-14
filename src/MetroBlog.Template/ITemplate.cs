using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetroBlog.Template
{
    public interface ITemplate
    {
        void Compile(string key, string content);
        void Compile(Views view);
        
        void Render(Views view, Stream sm);
        
        void Render(Views view, TextWriter writer);
    }
}
