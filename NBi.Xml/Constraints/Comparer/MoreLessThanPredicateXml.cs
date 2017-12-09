using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer
{
    public abstract class MoreLessThanPredicateXml : PredicateXml
    {
        [XmlAttribute("or-equal")]
        [DefaultValue(false)]
        public bool OrEqual { get; set; }
    }
}
