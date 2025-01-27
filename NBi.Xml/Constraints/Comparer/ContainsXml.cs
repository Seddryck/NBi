using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public class ContainsXml : ScalarReferencePredicateXml, ICaseSensitiveTextPredicateXml
{
    [XmlAttribute("ignore-case")]
    [DefaultValue(false)]
    public bool IgnoreCase { get; set; }

    public override ComparerType ComparerType { get => ComparerType.Contains; }
}
