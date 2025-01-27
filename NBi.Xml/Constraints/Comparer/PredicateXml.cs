using NBi.Core.Calculation;
using NBi.Xml.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public abstract class PredicateXml
{
    [DefaultValue(false)]
    [XmlAttribute("not")]
    public bool Not { get; set; }

    [XmlElement("projection")]
    public ProjectionOldXml? Projection { get; set; }

    [XmlElement("query-scalar")]
    public QueryScalarXml? QueryScalar { get; set; }

    [XmlIgnore]
    public abstract ComparerType ComparerType { get; }
}
