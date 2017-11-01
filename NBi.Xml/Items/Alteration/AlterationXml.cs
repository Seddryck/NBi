using NBi.Xml.Items.Calculation;
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
        public List<FilterXml> Filters { get; set; }

        public AlterationXml()
        {
            Filters = new List<FilterXml>();
        }
    }
}
