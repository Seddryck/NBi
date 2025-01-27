using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public class MatchesTimeXml : PredicateXml, ICultureSensitiveTextPredicateXml
{
    [XmlAttribute("culture")]
    public string? Culture { get; set; }
    public override ComparerType ComparerType { get => ComparerType.MatchesTime; }
}
