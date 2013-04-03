using System;
using System.Linq;
using NBi.Xml.Settings;

namespace NBi.Xml.Items
{
    public abstract class BaseItem
    {
        public DefaultXml Default { get; set; }
        public SettingsXml Settings { get; set; }
    }
}
