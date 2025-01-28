using NBi.Core.ResultSet;
using NBi.Xml.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Reshaping;

public class UnstackXml : AlterationXml
{
    [XmlElement("header")]
    public HeaderXml Header { get; set; }

    [XmlElement(ElementName = "group-by")]
    public GroupByXml GroupBy { get; set; }

    public UnstackXml() 
        => GroupBy = GroupByXml.None;

    [XmlIgnore]
    public bool GroupBySerialized
    {
        get => GroupBy != GroupByXml.None;
        set { throw new NotImplementedException(); }
    }

}
