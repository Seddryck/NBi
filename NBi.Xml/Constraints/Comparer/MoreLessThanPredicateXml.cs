using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public abstract class MoreLessThanPredicateXml : ScalarReferencePredicateXml, ICaseSensitiveTextPredicateXml
{
    [XmlAttribute("ignore-case")]
    [DefaultValue(false)]
    public bool IgnoreCase { get; set; }

    [XmlAttribute("or-equal")]
    [DefaultValue(false)]
    public bool OrEqual { get; set; }
}
