using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer
{
    public class EmptyXml : AbstractComparerXml
    {
        [XmlAttribute("or-null")]
        [DefaultValue(false)]
        public bool OrNull { get; set; }
    }
}
