using NBi.Xml.Items.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Sequence;

public class FilterSequenceXml
{
    [XmlElement("predicate")]
    public PredicationSequenceXml Predication { get; set; }
}
