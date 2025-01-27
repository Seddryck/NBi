using NBi.Core.ResultSet;
using NBi.Xml.Items.ResultSet;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints;

public class UniqueRowsXml : AbstractConstraintXml
{
    
    public UniqueRowsXml()
    {
        KeysSet = SettingsOrdinalResultSet.KeysChoice.All;
        ValuesSet = SettingsOrdinalResultSet.ValuesChoice.None;
    }

    [XmlAttribute("keys")]
    [DefaultValue(SettingsOrdinalResultSet.KeysChoice.All)]
    public SettingsOrdinalResultSet.KeysChoice KeysSet { get; set; }

    [XmlAttribute("values")]
    [DefaultValue(SettingsOrdinalResultSet.ValuesChoice.None)]
    public SettingsOrdinalResultSet.ValuesChoice ValuesSet { get; set; }

    [XmlElement("column")]
    public List<ColumnDefinitionXml> Columns { get; set; }

}
