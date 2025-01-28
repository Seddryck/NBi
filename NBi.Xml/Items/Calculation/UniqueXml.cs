using NBi.Xml.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation;

public class UniqueXml
{
    [XmlElement(ElementName = "group-by")]
    public GroupByXml GroupBy { get; set; }

    public UniqueXml() { }
}
