using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet.Lookup;

public class ColumnMappingXml
{
    [XmlAttribute("candidate")]
    public string Candidate { get; set; }
    [XmlAttribute("reference")]
    public string Reference { get; set; }
    [XmlAttribute("type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType Type { get; set; }

    [XmlAttribute("tolerance")]
    [DefaultValue("")]
    public string Tolerance { get; set; }
}
