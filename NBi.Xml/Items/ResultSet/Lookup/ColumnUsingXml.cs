using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet.Lookup;

public class ColumnUsingXml
{
    [XmlText]
    public string Column { get; set; }

    [XmlAttribute("type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType Type { get; set; }

    [XmlAttribute("tolerance")]
    [DefaultValue("")]
    public string Tolerance { get; set; }
}
