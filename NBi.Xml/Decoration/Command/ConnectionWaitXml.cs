using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using NBi.Core.Process;
using NBi.Xml.Settings;
using NBi.Core.Connection;

namespace NBi.Xml.Decoration.Command
{
    public class ConnectionWaitXml : DecorationCommandXml, IConnectionWaitCommand
    {
        [XmlAttribute("timeout-milliseconds")]
        [DefaultValue(60000)]
        public int TimeOut { get; set; }

        [XmlAttribute("connectionString")]
        public string SpecificConnectionString { get; set; }

        [XmlIgnore]
        public string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(SpecificConnectionString) && SpecificConnectionString.StartsWith("@"))
                    return Settings.GetReference(SpecificConnectionString.Remove(0, 1)).ConnectionString.GetValue();
                if (!String.IsNullOrWhiteSpace(SpecificConnectionString))
                    return SpecificConnectionString;
                if (Settings != null && Settings.GetDefault(SettingsXml.DefaultScope.Decoration) != null)
                    return Settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString.GetValue();
                return string.Empty;
            }
        }

        public ConnectionWaitXml()
        {
            TimeOut = 60000;
        }
    }
}
