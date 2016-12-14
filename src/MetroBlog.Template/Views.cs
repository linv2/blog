using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace MetroBlog.Template
{
    /// <summary>
    /// 页面
    /// </summary>
    public class Views
    {
        /// <summary>
        /// 测试正则
        /// </summary>
        public const string Testregexcode = "TESTREGEXCODE";

        readonly ThemesManage _themes = null;
        internal string ViewFilePath;
        public string ViewFile { get; set; }
        /// <summary>
        /// 访问Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 正则
        /// </summary>
        public string StrRegex { get; set; }

        public string ContentType { get; private set; } = "text/html";
        private Views(ThemesManage themes, string viewFilePath)
        {
            _themes = themes;
            ViewFilePath = viewFilePath;
        }

        internal static Views Parse(XmlNode viewNode, ThemesManage themeManage)
        {
            var viewElement = (XmlElement)viewNode;
            var viewUrl = viewElement.GetAttribute("Url");
            var viewFile = viewElement.GetAttribute("File");
            var viewRegex = viewElement.GetAttribute("Regex");
            var viewContentType = viewElement.GetAttribute("ContentType");
            if (!viewUrl.StartsWith("^"))
            {
                viewUrl = "^" + viewUrl;
            }
            if (!string.IsNullOrEmpty(viewUrl) && !string.IsNullOrEmpty(viewFile))
            {
                if (!string.IsNullOrEmpty(viewRegex))
                {
                    try
                    {
                        Regex.IsMatch(Testregexcode, viewRegex, RegexOptions.IgnoreCase);
                    }
                    catch { return null; }
                }
                var filePath = Path.Combine(themeManage.AbsolutePath, viewFile);
                if (!File.Exists(filePath))
                {
                    return null;
                }
                var view = new Views(themeManage, filePath);
                view.Url = viewUrl;
                view.StrRegex = viewRegex;
                view.ViewFile = viewFile;
                if (!string.IsNullOrEmpty(viewContentType))
                {
                    view.ContentType = viewContentType;
                }
                return view;
            }
            return null;
        }

        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileContent
        {
            get
            {
                var fileObj = HttpContext.Current.Cache.Get(ViewFilePath);
                return fileObj == null ? UpdateCache() : Convert.ToString(fileObj);
            }
        }
        private string UpdateCache()
        {
            string fileContent;
            lock (ViewFilePath)
            {
                fileContent = File.ReadAllText(ViewFilePath);
                HttpContext.Current.Cache.Add(string.Concat(_themes.ThemeName, Url), fileContent, new CacheDependency(ViewFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            return fileContent;
        }



        public void Render(Stream stream)
        {
            using (TextWriter tw = new StreamWriter(stream))
            {
                _themes.Template.Render(this, tw);
            }
        }

    }
}
