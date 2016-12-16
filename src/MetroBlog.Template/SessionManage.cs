using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web;

namespace MetroBlog.Template
{
    public class SessionManage
    {
        static readonly IDictionary<string, ThemesManage> ThemesDict = new ConcurrentDictionary<string, ThemesManage>();
        /// <summary>
        /// 当前访问的SessionId
        /// </summary>
        public static string SessionID
        {
            get
            {
                var context = HttpContext.Current;
                string sessionId;
                if (context.Request.Cookies["sessionId"] != null)
                {
                    sessionId = context.Request.Cookies["sessionId"].Value;
                }
                else
                {
                    sessionId = Guid.NewGuid().ToString("N");
                    var cookie = new HttpCookie("sessionId");
                    cookie.Value = sessionId;
                    cookie.HttpOnly = false;
                    cookie.Expires = DateTime.Now.AddYears(5);
                    context.Response.Cookies.Add(cookie);
                }
                return sessionId;
            }
        }


        /// <summary>
        /// 主题名
        /// </summary>
        public static string ThemeName { get; set; } = "Default";

        /// <summary>
        /// 当前访问用户的主题名
        /// </summary>
        public static string PreviewThemeName
        {
            get
            {
                var context = HttpContext.Current;
                var themeName = ThemeName;
                if (context.Request.Cookies["themeName"] != null)
                {
                    themeName = context.Request.Cookies["themeName"].Value;
                }
                return themeName;
            }
        }
        private static object lockObj = new object();
        /// <summary>
        /// 当前访问用户的主题
        /// </summary>
        public static ThemesManage CurrentTheme
        {
            get
            {
                var themeName = PreviewThemeName;
                ThemesManage currentTheme = null;
                if (string.IsNullOrEmpty(themeName)) return null;
                if (ThemesDict.TryGetValue(themeName, out currentTheme)) return currentTheme;
                lock (lockObj)
                {
                    var themeManage = ThemesManage.Create(themeName);
                    if (themeManage != null)
                    {
                        try
                        {
                            ThemesDict.Add(themeName, themeManage);
                        }
                        catch { }
                    }
                    currentTheme = themeManage;
                }
                return currentTheme;
            }
        }
    }
}
