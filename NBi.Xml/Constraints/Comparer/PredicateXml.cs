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
        public virtual string Value { get; set; }

        public virtual bool ShouldSerializeValue() => true;

        [DefaultValue(false)]
        [XmlAttribute("not")]
        public bool Not { get; set; }

        [XmlElement("item")]
        public List<string> Values { get; set; }

        [XmlElement("projection")]
        public ProjectionOldXml Projection { get; set; }

        [XmlElement("query-scalar")]
        public QueryScalarXml QueryScalar { get; set; }

        [XmlIgnore]
        internal abstract ComparerType ComparerType { get; }
    }
}
