using NBi.Xml.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Summarization;

public class SummarizeXml : AlterationXml
{
    [XmlElement(Type = typeof(SumXml), ElementName = "sum"),
        XmlElement(Type = typeof(MinXml), ElementName = "min"),
        XmlElement(Type = typeof(MaxXml), ElementName = "max"),
        XmlElement(Type = typeof(AverageXml), ElementName = "average"),
        XmlElement(Type = typeof(CountRowsXml), ElementName = "count"),
        XmlElement(Type = typeof(ConcatenationXml), ElementName = "concatenation"),
    ]
    public AggregationXml Aggregation { get; set; }

    [XmlElement(ElementName = "group-by")]
    public GroupByXml GroupBy { get; set; }

}
