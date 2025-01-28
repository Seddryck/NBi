using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet;

public class EmptyResultSetXml
{
    [XmlAttribute("column-count")]
    [DefaultValue("")]
    public string ColumnCount { get; set; }

    [XmlElement("column")]
    public List<ColumnDefinitionLightXml> Columns { get; set; } = new List<ColumnDefinitionLightXml>();

    [XmlIgnore]
    public bool ColumnsSpecified
    {
        get => (Columns?.Count ?? 0) != 0;
        set { return; }
    }
}
