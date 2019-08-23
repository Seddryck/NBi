using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Sequence.Transformation
{
    public class SummarizationXml
    {
        [XmlElement(Type = typeof(SumXml), ElementName = "sum"),
            XmlElement(Type = typeof(MinXml), ElementName = "min"),
            XmlElement(Type = typeof(MaxXml), ElementName = "max"),
            XmlElement(Type = typeof(AverageXml), ElementName = "average"),
        ]
        public AggregationXml Aggregation { get; set; }
    }
}
