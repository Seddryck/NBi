using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Grouping;

[XmlInclude(typeof(GroupByNone))]
public class GroupByXml
{
    [XmlElement("column")]
    public List<ColumnDefinitionLightXml> Columns { get; set; } = new List<ColumnDefinitionLightXml>();
    
    [XmlElement("case")]
    public List<CaseXml> Cases { get; set; }

    public static GroupByXml None { get; } = new GroupByNone();

    public class GroupByNone : GroupByXml { }
}
