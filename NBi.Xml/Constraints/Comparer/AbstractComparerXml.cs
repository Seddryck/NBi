using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer
{
    public abstract class AbstractComparerXml
    {
        [XmlText]
        public int Value { get; set; }

        [XmlAttribute("not")]
        [DefaultValue(false)]
        public bool Not { get; set; }
    }
}
