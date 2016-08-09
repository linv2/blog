using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MetroBlog.Settings
{
    public class SettingItem
    {
        private SettingItem()
        {
            this.ItemType = ItemType.INPUT;
        }
        public String DisplayText { get; set; }
        public String ItemName { get; set; }
        public Object Default { get; set; }
        public Type DateType { get; set; }
        public ItemType ItemType { get; set; }
        public List<SettingItemOptions> Options { get; set; }

        public static SettingItem Parse(XmlNode node)
        {
            XmlElement element = (XmlElement)node;

            var item = new SettingItem();
            item.DisplayText = element.GetAttribute("display");
            item.ItemName = element.GetAttribute("name");
            item.DateType = Type.GetType(element.GetAttribute("dateType"), false);
            var _default = element.GetAttribute("default");
            if (!String.IsNullOrEmpty(_default))
            {
                item.Default = Convert.ChangeType(_default, item.DateType);
            }
            var _itemType = element.GetAttribute("itemType");
            if (!String.IsNullOrEmpty(_itemType))
            {
                try
                {
                    item.ItemType = (ItemType)Enum.Parse(typeof(ItemType), _itemType, true);
                }
                catch { }
            }
            return item;
        }
    }
}
