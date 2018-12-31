using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Lookup;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration
{
    public class AlterationXml
    {
        [XmlElement("filter")]
        public List<FilterXml> Filters { get; set; } = new List<FilterXml>();
        [XmlElement("convert")]
        public List<ConvertXml> Conversions { get; set; } = new List<ConvertXml>();
        [XmlElement("transform")]
        public List<TransformXml> Transformations { get; set; } = new List<TransformXml>();
        [XmlElement("lookup")]
        public List<LookupXml> Lookups { get; set; } = new List<LookupXml>();
    }
}
