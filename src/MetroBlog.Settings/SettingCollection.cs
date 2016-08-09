using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MetroBlog.Settings
{
    public class SettingCollection
    {
        private SettingCollection()
        {

        }
        public String TypeName { get; set; }
        public List<SettingItem> SettingItems { get; set; }

        public static SettingCollection Parse(XmlNode node)
        {
            XmlElement element = (XmlElement)node;
            var collection = new SettingCollection();
            collection.SettingItems = new List<SettingItem>();


            collection.TypeName = element.GetAttribute("name");
            var nodeList = node.SelectNodes("item");
            foreach (XmlNode nodeItem in nodeList)
            {
                collection.SettingItems.Add(SettingItem.Parse(nodeItem));
            }
            return collection;
        }
    }
}
