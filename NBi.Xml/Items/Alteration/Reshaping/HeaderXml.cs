using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Reshaping;

public class HeaderXml
{
    [XmlElement("column")]
    public ColumnDefinitionLightXml Column { get; set; }

    [XmlElement("enforced-value")]
    public List<string> EnforcedValues { get; set; } = new List<string>();
}
