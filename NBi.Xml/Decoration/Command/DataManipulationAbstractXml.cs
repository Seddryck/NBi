using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.DataManipulation;
using NBi.Xml.Settings;

namespace NBi.Xml.Decoration.Command
{
    public class DataManipulationAbstractXml : DecorationCommandXml, IDataManipulationCommand
    {
        [XmlAttribute("connectionString")]
        public string SpecificConnectionString { get; set; }


        public string ConnectionString
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(SpecificConnectionString))
                    return SpecificConnectionString;
                if (Settings != null && Settings.GetDefault(SettingsXml.DefaultScope.Decoration) != null)
                    return Settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString;
                return string.Empty;
            }
        }
    }
}
