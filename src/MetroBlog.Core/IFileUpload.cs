using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace MetroBlog.Core
{

    public interface IFileUpload
    {
        UploadResult Execute(IEnumerable<HttpFile> httpFile);

    }
    public class UploadResult
    {
        public UploadResult()
        {
        }

        public UploadResult(string message)
        {
            this.success = 0;
            this.message = message;
        }

        public int success { get; set; } = 1;
        public string message { get; set; }
        public string url { get; set; }
    }


}
