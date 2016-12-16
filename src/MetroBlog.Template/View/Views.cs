using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace MetroBlog.Template.View
{
    public abstract class Views
    {
        /// <summary>
        /// 测试正则
        /// </summary>
        public const string TestrRegexCode = "TESTREGEXCODE";
        protected ThemesManage _themes = null;
        internal string ViewFilePath { get; set; }
        public string ContentType { get; private set; } = "text/html";

        public int Level { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string RequestIndex { get; set; }
        


        internal static Views Parse(XmlNode viewNode, ThemesManage themeManage)
        {
            Views view = null;
            var viewElement = (XmlElement)viewNode;
            var viewUrl = viewElement.GetAttribute("Url");
            var viewFile = viewElement.GetAttribute("File");
            var viewRegex = viewElement.GetAttribute("Regex");
            var viewContentType = viewElement.GetAttribute("ContentType");
            var viewCode = viewElement.GetAttribute("Code");
            var viewLevel = viewElement.GetAttribute("Level");
            if (!string.IsNullOrEmpty(viewCode))
            {
                view = new CustomView();
                view.RequestIndex = viewCode;
            }
            else
            {

                if (!string.IsNullOrEmpty(viewUrl))
                {
                    view = new PageView();
                    view.RequestIndex = viewUrl;
                }
                else if (!string.IsNullOrEmpty(viewRegex))
                {
                    view = new RegexView();
                    if (!string.IsNullOrEmpty(viewRegex))
                    {
                        try
                        {
                            Regex.IsMatch(TestrRegexCode, viewRegex, RegexOptions.IgnoreCase);
                            view.RequestIndex = viewRegex;
                        }
                        catch { view = null; }
                    }
                }
            }
            if (view == null)
            {
                return null;
            }
            if (Regex.IsMatch(viewLevel, @"^\d+"))
            {
                view.Level += Convert.ToInt32(viewLevel);
            }
            var filePath = Path.Combine(themeManage.AbsolutePath, viewFile);
            if (!File.Exists(filePath))
            {
                return null;
            }
            view.ViewFilePath = filePath;
            view._themes = themeManage;
            if (!string.IsNullOrEmpty(viewContentType))
            {
                view.ContentType = viewContentType;
            }
            return view;
        }  /// <summary>
           /// 文件内容
           /// </summary>
        public string FileContent
        {
            get
            {
                var fileObj = HttpContext.Current.Cache.Get(ViewFilePath);
                return fileObj == null ? UpdateViewContent() : Convert.ToString(fileObj);
            }
        }
        public string UpdateViewContent()
        {
            string fileContent;
            lock (ViewFilePath)
            {
                fileContent = File.ReadAllText(ViewFilePath);
                HttpContext.Current.Cache.Add(string.Concat(_themes.ThemeName, RequestIndex), fileContent, new CacheDependency(ViewFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            return fileContent;
        }

        public abstract bool Match(Uri requestUri, string requestIndex = null);
    }
}
