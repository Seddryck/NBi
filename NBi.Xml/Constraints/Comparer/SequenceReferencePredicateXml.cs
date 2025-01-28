using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints.Comparer;

public abstract class SequenceReferencePredicateXml : ReferencePredicateXml
{
    [XmlElement("item")]
    public List<string> References { get; set; }
}
