using NBi.Core.Calculation;
using NBi.Xml.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer
{
    public abstract class PredicateXml
    {
        [XmlText]
        public string Value { get; set; }

        [XmlElement("projection")]
        public ProjectionXml Projection { get; set; }

        [XmlElement("scalar-query")]
        public ScalarQueryXml ScalarQuery { get; set; }

        [XmlIgnore]
        internal abstract ComparerType ComparerType { get; }
    }
}
