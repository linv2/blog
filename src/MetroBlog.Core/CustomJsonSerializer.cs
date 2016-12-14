using Nancy;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetroBlog.Core
{
    public class CustomJsonSerializer : ISerializer
    {
        static readonly IsoDateTimeConverter DateConverter = new IsoDateTimeConverter();
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

                var json = JsonConvert.SerializeObject(model, DateConverter);
                streamWriter.Write(json);
            }
        }
    }
}

