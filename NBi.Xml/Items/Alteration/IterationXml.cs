using NBi.Xml.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration;

public class IterationXml
{
    [XmlElement("sequence")]
    public SequenceXml Sequence { get; set; }
}
