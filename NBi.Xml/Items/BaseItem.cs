using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Items
{
    public abstract class BaseItem
    {
        public DefaultXml Default { get; set; }
        public SettingsXml Settings { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public virtual string GetConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString) && ConnectionString.StartsWith("@"))
                return Settings.GetReference(ConnectionString.Remove(0, 1)).ConnectionString;

            //Else get the ConnectionString as-is
            //if ConnectionString is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the default ConnectionString 
            if (Default != null && !string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }
    }
}
