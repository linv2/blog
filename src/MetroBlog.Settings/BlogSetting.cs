using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

namespace MetroBlog.Settings
{
    public class BlogSetting
    {
        private Dictionary<String, BlogSettingItem> settinItemgDict = new Dictionary<string, BlogSettingItem>();
        public BlogSettingItem this[string key]
        {
            get
            {
                BlogSettingItem outValue = null;
                if (settinItemgDict.TryGetValue(key, out outValue))
                {
                    return outValue;
                }
                return null;
            }
            set
            {
                if (String.IsNullOrEmpty(key))
                {
                    return;
                }
                if (settinItemgDict.ContainsKey(key))
                {
                    settinItemgDict[key] = value;
                }
                else
                {
                    settinItemgDict.Add(key, value);
                }
            }
        }

        static object lockObj = new object();
        static BlogSetting _instance = null;
        public static BlogSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        _instance = new BlogSetting();
                    }
                }
                return _instance;
            }
        }

        public static void Register(string key,string configTemplate, string settingFile) {
            var settingItem = new BlogSettingItem(configTemplate, settingFile);
            settingItem.LoadConfig();
            Instance[key] = settingItem;
        }
    }
}
