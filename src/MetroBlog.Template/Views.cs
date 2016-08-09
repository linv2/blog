using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public const string TESTREGEXCODE = "TESTREGEXCODE";
        ThemesManage themes = null;
        internal string viewFilePath = string.Empty;
        private Views(ThemesManage themes, string viewFilePath)
        {
            this.themes = themes;
            this.viewFilePath = viewFilePath;
        }

        internal static Views Parse(XmlNode viewNode, ThemesManage themeManage)
        {
            var viewElement = (XmlElement)viewNode;
            var viewUrl = viewElement.GetAttribute("Url");
            var viewFile = viewElement.GetAttribute("File");
            var viewRegex = viewElement.GetAttribute("Regex");
            if (!viewUrl.StartsWith("^"))
            {
                viewUrl = "^" + viewUrl;
            }
            if (!String.IsNullOrEmpty(viewUrl) && !String.IsNullOrEmpty(viewFile))
            {
                if (!String.IsNullOrEmpty(viewRegex))
                {
                    try
                    {
                        Regex.IsMatch(TESTREGEXCODE, viewRegex, RegexOptions.IgnoreCase);
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
                view.strRegex = viewRegex;
                view.ViewFile = viewFile;
                return view;
            }
            return null;
        }
        public string ViewFile { get; set; }
        /// <summary>
        /// 访问Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 正则
        /// </summary>
        public string strRegex { get; set; }
        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileContent
        {
            get
            {
                var fileObj = HttpContext.Current.Cache.Get(viewFilePath);
                if (fileObj == null)
                {
                    return UpdateCache();
                }
                else
                {
                    return Convert.ToString(fileObj);
                }
            }
        }
        private String UpdateCache()
        {
            var _fileContent = string.Empty;
            lock (viewFilePath)
            {
                _fileContent = File.ReadAllText(viewFilePath);
                HttpContext.Current.Cache.Add(String.Concat(this.themes.themeName, this.Url), _fileContent, new CacheDependency(this.viewFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            return _fileContent;
        }



        public void Render(Stream stream)
        {
            using (TextWriter tw = new StreamWriter(stream))
            {
                themes.Template.Run(this, tw);
            }
        }

    }
}
