using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core
{
    public class CoreIoCContainer
    {
        public static TinyIoCContainer Current { get; set; }
    }
}
