using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public abstract class ScalarReferencePredicateXml : ReferencePredicateXml
{
    [XmlText]
    public string? Reference { get; set; }

    public virtual bool ShouldSerializeReference() => true;
}
