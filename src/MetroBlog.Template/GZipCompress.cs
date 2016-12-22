using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MetroBlog.Template
{
    public class GZipCompress
    {

        public static void CheckForCompression(HttpContext context)
        {
            if (!RequestIsGzipCompatible(context))
            {
                return;
            }

            if (context.Response.StatusCode != 200)
            {
                return;
            }

            if (!ResponseIsCompatibleMimeType(context))
            {
                return;
            }

            if (ContentLengthIsTooSmall(context))
            {
                return;
            }

            CompressResponse(context);
        }

        private static void CompressResponse(HttpContext context)
        {
            context.Response.Headers["Content-Encoding"] = "gzip";
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
        }

        private static bool ContentLengthIsTooSmall(HttpContext context)
        {
            string contentLength = context.
            Response.Headers["Content-Length"];
            if (!string.IsNullOrEmpty(contentLength))
            {
                var length = long.Parse(contentLength);
                if (length < 4096)
                {
                    return true;
                }
            }
            return false;
        }


        public static List<string> ValidMimes = new List<string>
                                                {
                                                    "text/css",
                                                    "text/html",
                                                    "text/plain",
                                                    "application/xml",
                                                    "application/json",
                                                    "application/xaml+xml",
                                                    "application/x-javascript"
                                                };

        private static bool ResponseIsCompatibleMimeType(HttpContext context)
        {
            return ValidMimes.Any(x => x == context.Response.ContentType);
        }
        private static bool RequestIsGzipCompatible(HttpContext context)
        {
            var acceptEncoding = context.Request.Headers["Accept-Encoding"];
            return !string.IsNullOrEmpty(acceptEncoding) && acceptEncoding.Contains("gzip");
        }
    }
}
