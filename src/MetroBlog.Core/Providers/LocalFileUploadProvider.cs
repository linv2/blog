using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace MetroBlog.Core.Providers
{
    public class LocalFileUploadProvider : IFileUpload
    {
        private readonly string[] supportExtension = { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };
        private readonly string FilePath;
        private readonly string VirtualPath;

        public LocalFileUploadProvider()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Blog.Current.Setting.UploadPath);
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            VirtualPath = string.Concat("//", Blog.Current.Setting.Host, "/", Blog.Current.Setting.UploadPath);
        }
        public UploadResult Execute(IEnumerable<HttpFile> httpFiles)
        {
            if (httpFiles.Count() != 1)
            {
                return new UploadResult("不支持多文件上传");
            }
            var httpFile = httpFiles.FirstOrDefault();
            var fileExtension = System.IO.Path.GetExtension(httpFile.Name);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return new UploadResult("不支持的文件格式");
            }
            if (!supportExtension.Contains(fileExtension))
            {
                return new UploadResult("不支持的文件格式");
            }
            var format = DateTime.Now.ToString("yyyyMMdd");
            var filePath = Path.Combine(FilePath, format);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = string.Concat(getFileName, fileExtension);
            var fieVirtualPath = string.Concat(VirtualPath, "/", format, "/", fileName);
            fileName = Path.Combine(filePath, fileName);
            using (var streamWrite = new StreamWriter(fileName))
            {
                var bytes = new byte[httpFile.Value.Length];
                httpFile.Value.Read(bytes, 0, bytes.Length);
                streamWrite.BaseStream.Write(bytes, 0, bytes.Length);
                streamWrite.Flush();
            }
            return new UploadResult()
            {
                url = fieVirtualPath
            };
        }

        public string getFileName
        {
            get
            {
                lock (supportExtension)
                {
                    return Guid.NewGuid().ToString("N");
                }
            }
        }
    }
}

