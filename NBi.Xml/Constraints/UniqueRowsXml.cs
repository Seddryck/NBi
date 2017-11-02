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
        }

        [XmlAttribute("keys")]
        [DefaultValue(SettingsResultSetComparisonByIndex.KeysChoice.All)]
        public SettingsResultSetComparisonByIndex.KeysChoice KeysSet { get; set; }

        [XmlAttribute("values")]
        [DefaultValue(SettingsResultSetComparisonByIndex.ValuesChoice.None)]
        public SettingsResultSetComparisonByIndex.ValuesChoice ValuesSet { get; set; }

        [XmlElement("column")]
        public List<ColumnDefinitionXml> Columns { get; set; }

    }
}
