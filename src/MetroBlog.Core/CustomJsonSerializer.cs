using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetroBlog.Core
{
    class CustomJsonSerializer : ISerializer
    {
        static IsoDateTimeConverter dateConverter = new IsoDateTimeConverter()
        {
            
        };
        public IEnumerable<string> Extensions
        {
            get { yield return "*"; }
        }

        public bool CanSerialize(string contentType)
        {
            return true;
        }

        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            {

                var json = JsonConvert.SerializeObject(model, dateConverter);
                streamWriter.Write(json);
            }
        }
    }
}

