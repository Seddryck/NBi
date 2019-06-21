using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Sequence.Transformation
{
    public class ProjectionXml
    {
        [XmlElement(Type = typeof(SumXml), ElementName = "sum"),
            XmlElement(Type = typeof(MinXml), ElementName = "min"),
            XmlElement(Type = typeof(MaxXml), ElementName = "max"),
            XmlElement(Type = typeof(MeanXml), ElementName = "mean"),
        ]
        public AggregationXml Aggregation { get; set; }
    }
}
