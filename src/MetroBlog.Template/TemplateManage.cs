using MetroBlog.Settings;
using MetroBlog.Template.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace MetroBlog.Template
{
    public class ThemesManage
    {
        /// <summary>
        /// 绝对路径
        /// </summary>
        public string AbsolutePath
        {
            get;
            private set;
        }
        /// <summary>
        /// 模板名字
        /// </summary>
        public string themeName { get; private set; }

        #region 页面数据
        Dictionary<string, Views> _urlDict = new Dictionary<string, Views>();
        private string[] _allKeys = new string[] { };
        /// <summary>
        /// 页面总数量
        /// </summary>
        public int Count
        {
            get
            {
                return _urlDict.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String[] AllKeys
        {
            get
            {
                return _allKeys;
            }
        }
        public Views this[string viewUrl]
        {
            get
            {
                if (String.IsNullOrEmpty(viewUrl))
                {
                    return null;
                }
                Views value = null;
                if (!_urlDict.TryGetValue(viewUrl, out value))
                {
                    return null;
                }
                return value;

            }
        }
        #endregion 页面数据

        public RazorTemplate Template = null;
        public ThemesManage(string themeName)
        {
            this.themeName = themeName;
        }
        public static ThemesManage Create(string themeName)
        {
            try
            {
                var _themeManage = new ThemesManage(themeName);
                _themeManage.ParseThemeConfig();
                return _themeManage;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void ParseThemeConfig()
        {
            lock (this)
            {
                this.AbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", themeName);
                var configFile = Path.Combine(AbsolutePath, "theme.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);
                var viewNodeList = xmlDoc.SelectNodes("config/Views/View");
                List<Views> themeConfigList = new List<Views>();
                foreach (XmlNode viewNode in viewNodeList)
                {
                    var view = Views.Parse(viewNode, this);
                    if (view != null)
                    {
                        themeConfigList.Add(view);
                    }

                }
                Template = new RazorTemplate();
                foreach (var configEntity in themeConfigList)
                {
                    if (!_urlDict.ContainsKey(configEntity.Url))
                    {
                        _urlDict.Add(configEntity.Url, configEntity);
                        Template.Compile(configEntity);

                    }
                }
                _allKeys = _urlDict.Select(x => x.Key).ToArray();
                _allKeys = _allKeys.OrderByDescending(x => x.Length).ToArray();

                var layoutNodeList = xmlDoc.SelectNodes("config/Layouts/Layout");
                foreach (XmlNode viewNode in layoutNodeList)
                {
                    try
                    {
                        var path = viewNode.Attributes["Path"].Value;
                        Template.Compile(path, File.ReadAllText(Path.Combine(this.AbsolutePath, path)));
                    }
                    catch { }
                }
            }
        }
        /// <summary>
        /// 根据访问路径查找View
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public Views FindView(string absolutePath)
        {
            var view = this[absolutePath];
            if (view == null)
            {
                foreach (var key in this.AllKeys)
                {
                    if (Regex.IsMatch(absolutePath, key, RegexOptions.IgnoreCase))
                    {
                        var absolutePathEndChr = absolutePath.EndsWith("/");
                        if (key.EndsWith("/") && (!absolutePathEndChr || (absolutePath != "/" && absolutePathEndChr)))
                        {
                            continue;
                        }
                        view = this[key];
                        break;
                    }
                }
            }
            return view;
        }
    }
}
