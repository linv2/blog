using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using MetroBlog.Template.View;

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

        private IList<Views> _viewList = new List<Views>();
        /// <summary>
        /// 页面总数量
        /// </summary>
        public int Count => _viewList.Count;

        public Views this[Uri requestUri, string requestIndex = null]
        {
            get
            {
                foreach (var view in _viewList)
                {
                    if (view.Match(requestUri, requestIndex))
                    {
                        return view;
                    }
                }
                return null;
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
                #region parse engine
                var engine = xmlDoc.SelectSingleNode("config/Engine");
                LoadEngine(engine.InnerText);
                #endregion
                #region parse View
                var viewNodeList = xmlDoc.SelectNodes("config/Views/View");
                var themeConfigList = new List<Views>();
                if (viewNodeList != null)
                    foreach (XmlNode viewNode in viewNodeList)
                    {
                        var view = Views.Parse(viewNode, this);
                        if (view != null)
                        {
                            _viewList.Add(view);
                            Template.Compile(view);
                        }

                    }
                _viewList = _viewList.OrderByDescending(x => x.Level).ToList();

                #endregion parse View
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
                            Template.Compile(path, File.ReadAllText(Path.Combine(AbsolutePath, path)));
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
        /// <param name="requestUri"></param>
        /// <param name="requestIndex"></param>
        /// <returns></returns>
        public Views FindView(Uri requestUri, string requestIndex = null)
        {
            return this[requestUri, requestIndex];
        }
    }
}
