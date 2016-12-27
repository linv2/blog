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
        private readonly string[] _supportExtension = { ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".zip", ".exe" };
        private readonly string _filePath;
        private readonly string _virtualPath;

        public LocalFileUploadProvider()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Blog.Current.Setting.UploadPath);
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
            _virtualPath = string.Concat("//", Blog.Current.Setting.Host, "/", Blog.Current.Setting.UploadPath);
        }
        public UploadResult Execute(IEnumerable<HttpFile> httpFiles)
        {
            if (httpFiles.Count() != 1)
            {
                return new UploadResult("不支持多文件上传");
            }
            var httpFile = httpFiles.FirstOrDefault();
            var fileExtension = Path.GetExtension(httpFile.Name);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return new UploadResult("不支持的文件格式");
            }
            if (!_supportExtension.Contains(fileExtension))
            {
                return new UploadResult("不支持的文件格式");
            }
            var format = DateTime.Now.ToString("yyyyMMdd");
            var filePath = Path.Combine(_filePath, format);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = string.Concat(getFileName, fileExtension);
            var fieVirtualPath = string.Concat(_virtualPath, "/", format, "/", fileName);
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
                lock (_supportExtension)
                {
                    return Guid.NewGuid().ToString("N");
                }
            }
        }
    }
}

