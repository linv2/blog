using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace MetroBlog.Settings
{
  public  class BlogSettingItem
    {
        internal BlogSettingItem(string configTemplate, string settingFile)
        {
            this.configTemplate = configTemplate;
            this.configPath = settingFile;
            this.Load();
            
        }
        public string[] AllKey
        {
            get
            {
                return this.settingItemDict.Select(x => x.Key).ToArray();
            }
        }
        public object this[string key]
        {
            get
            {
                object outValue = null;
                if (settingDict.TryGetValue(key, out outValue))
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
                if (settingDict.ContainsKey(key))
                {
                    settingDict[key] = value;
                }
                else
                {
                    settingDict.Add(key, value);
                }
            }
        }
        private Dictionary<String, Object> settingDict = new Dictionary<string, object>();
        private Dictionary<String, SettingItem> settingItemDict = new Dictionary<string, SettingItem>();
        public List<SettingCollection> SettingTypes { get; set; }

        public void SetConfigPath(String configPath)
        {
            if (!String.IsNullOrEmpty(configPath))
            {
                this.configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", configPath);
            }
            if (File.Exists(this.configPath))
            {
                File.Create(this.configPath);
            }
        }
        private String configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/setting.config");
        private string configTemplate = null;
        public void LoadConfig()
        {
            if (File.Exists(this.configPath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(this.configPath);
                    var nodeList = xmlDoc.SelectNodes("root/item");
                    foreach (XmlNode nodeItem in nodeList)
                    {
                        var element = (XmlElement)nodeItem;
                        var settingKey = element.GetAttribute("name");
                        var settingValue = element.InnerXml;
                        SettingItem _settingItem = null;
                        if (settingItemDict.TryGetValue(settingKey, out _settingItem))
                        {
                            this[settingKey] = Convert.ChangeType(settingValue, _settingItem.DateType);
                        }
                        else
                        {
                            this[settingKey] = settingValue;
                        }
                    }
                }
                catch { }
            }
        }
        public void SaveConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            var rootElement = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(rootElement);

            foreach (var settingItem in this.settingDict)
            {
                var settingElement = xmlDoc.CreateElement("item");
                settingElement.SetAttribute("name", settingItem.Key);
                settingElement.InnerText = Convert.ToString(settingItem.Value);
                rootElement.AppendChild(settingElement);
            }
            xmlDoc.Save(this.configPath);
            LoadConfig();
        }
        private void Load()
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configTemplate);
            ParseSetting(xmlDoc);
            this.SettingTypes.ForEach(settingType =>
            {
                settingType.SettingItems.ForEach(settingItem =>
                {
                    if (!settingItemDict.ContainsKey(settingItem.ItemName))
                    {
                        settingItemDict.Add(settingItem.ItemName, settingItem);
                        this[settingItem.ItemName] = settingItem.Default;
                    }
                });
            });
        }
        private void ParseSetting(XmlDocument xmlDoc)
        {
            SettingTypes = new List<SettingCollection>();
            var collectionNode = xmlDoc.SelectNodes("setting/collection");
            foreach (XmlNode nodeItem in collectionNode)
            {
                SettingTypes.Add(SettingCollection.Parse(nodeItem));
            }
        }

        public dynamic Dynamic
        {
            get
            {
                return settingDict.Select(x => new
                {
                    x.Key,
                    Value = Convert.ToString(x.Value)
                });
            }
        }


        static object lockObj = new object();
    }
}
