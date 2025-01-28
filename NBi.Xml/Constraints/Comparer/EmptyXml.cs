using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public class EmptyXml : PredicateXml
{
    [XmlAttribute("or-null")]
    [DefaultValue(false)]
    public bool OrNull { get; set; }

    public override ComparerType ComparerType { get => OrNull ? ComparerType.NullOrEmpty : ComparerType.Empty; }
}
