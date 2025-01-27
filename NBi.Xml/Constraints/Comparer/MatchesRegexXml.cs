using NBi.Core.Calculation;
using NBi.Xml.SerializationOption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public class MatchesRegexXml : ScalarReferencePredicateXml, ICaseSensitiveTextPredicateXml
{
    [XmlAttribute("ignore-case")]
    [DefaultValue(false)]
    public bool IgnoreCase { get; set; }

    [XmlIgnore]
    public CData ValueWrite { get => Reference; set => Reference = value; }

    public override bool ShouldSerializeReference() => false;

    public override ComparerType ComparerType { get => ComparerType.MatchesRegex; }
}
