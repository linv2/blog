using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public string ThemeName { get; private set; }

        #region 页面数据

        readonly Dictionary<string, Views> _urlDict = new Dictionary<string, Views>();
        private string[] _allKeys = new string[] { };
        /// <summary>
        /// 页面总数量
        /// </summary>
        public int Count => _urlDict.Count;

        /// <summary>
        /// 
        /// </summary>
        public string[] AllKeys => _allKeys;

        public Views this[string viewUrl]
        {
            get
            {
                if (string.IsNullOrEmpty(viewUrl))
                {
                    return null;
                }
                Views value = null;
                return !_urlDict.TryGetValue(viewUrl, out value) ? null : value;
            }
        }
        #endregion 页面数据

        public ITemplate Template = null;
        public ThemesManage(string themeName)
        {
            this.ThemeName = themeName;

        }
        public static ThemesManage Create(string themeName)
        {
            try
            {
                var themeManage = new ThemesManage(themeName);
                themeManage.ParseThemeConfig();
                return themeManage;
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
                this.AbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", ThemeName);
                var configFile = Path.Combine(AbsolutePath, "theme.xml");
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);

                var engine = xmlDoc.SelectSingleNode("config/Engine");
                LoadEngine(engine.InnerText);
                #region parse View
                var viewNodeList = xmlDoc.SelectNodes("config/Views/View");
                var themeConfigList = new List<Views>();
                if (viewNodeList != null)
                    foreach (XmlNode viewNode in viewNodeList)
                    {
                        var view = Views.Parse(viewNode, this);
                        if (view != null)
                        {
                            themeConfigList.Add(view);
                        }

                    }

                foreach (var configEntity in themeConfigList)
                {
                    if (_urlDict.ContainsKey(configEntity.Url)) continue;
                    _urlDict.Add(configEntity.Url, configEntity);
                    Template.Compile(configEntity);
                }
                #endregion parse View
                _allKeys = _urlDict.Select(x => x.Key).ToArray();
                _allKeys = _allKeys.OrderByDescending(x => x.Length).ToArray();
                #region parse Layout
                var layoutNodeList = xmlDoc.SelectNodes("config/Layouts/Layout");
                if (layoutNodeList == null || layoutNodeList.Count == 0) return;
                {
                    foreach (XmlNode viewNode in layoutNodeList)
                    {
                        try
                        {
                            if (viewNode.Attributes == null) continue;
                            var path = viewNode.Attributes["Path"].Value;
                            Template.Compile(path, File.ReadAllText(Path.Combine(this.AbsolutePath, path)));
                        }
                        catch { }
                    }
                }
                #endregion

            }
        }

        private void LoadEngine(string engine)
        {
            var assemblyFile = string.Concat("MetroBlog.Template.", engine, ".dll");
            var assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", assemblyFile));

            var templateType = typeof(ITemplate);
            var templateImplType = assembly.GetTypes()
                  .FirstOrDefault(x => x.GetInterface(templateType.FullName, false) == templateType);
            Template = (ITemplate)Activator.CreateInstance(templateImplType, null);

        }
        /// <summary>
        /// 根据访问路径查找View
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public Views FindView(string absolutePath)
        {
            var view = this[absolutePath];
            if (view != null) return view;
            foreach (var key in this.AllKeys)
            {
                if (!Regex.IsMatch(absolutePath, key, RegexOptions.IgnoreCase)) continue;
                var absolutePathEndChr = absolutePath.EndsWith("/");
                if (key.EndsWith("/") && (!absolutePathEndChr || (absolutePath != "/" && absolutePathEndChr)))
                {
                    continue;
                }
                view = this[key];
                break;
            }
            return view;
        }
    }
}
