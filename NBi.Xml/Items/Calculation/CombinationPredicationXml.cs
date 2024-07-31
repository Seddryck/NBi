using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation
{
    public class CombinationPredicationXml : AbstractPredicationXml
    {
        [XmlAttribute("operator")]
        public CombinationOperator Operator { get; set; }

        [DefaultValue(false)]
        [XmlAttribute("not")]
        public bool Not { get; set; }

        [XmlElement("predicate")]
        public List<SinglePredicationXml> Predications { get; set; } = [];
    }
}
