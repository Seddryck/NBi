using NBi.Core.ResultSet;
using NBi.Xml.Items.ResultSet;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class UniqueRowsXml : AbstractConstraintXml
    {
        
        public UniqueRowsXml()
        {
            KeysSet = SettingsIndexResultSet.KeysChoice.All;
            ValuesSet = SettingsIndexResultSet.ValuesChoice.None;
        }

        [XmlAttribute("keys")]
        [DefaultValue(SettingsIndexResultSet.KeysChoice.All)]
        public SettingsIndexResultSet.KeysChoice KeysSet { get; set; }

        [XmlAttribute("values")]
        [DefaultValue(SettingsIndexResultSet.ValuesChoice.None)]
        public SettingsIndexResultSet.ValuesChoice ValuesSet { get; set; }

        [XmlElement("column")]
        public List<ColumnDefinitionXml> Columns { get; set; }

    }
}
