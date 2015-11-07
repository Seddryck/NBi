using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Validate
{
    public class FormulaXml
    {
        [XmlText()]
        public string Value { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
