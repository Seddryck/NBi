using NBi.Core.ResultSet;
using NBi.Xml.Items.Calculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Sequence;

public class PredicationSequenceXml : BasePredicationXml
{
    [XmlAttribute("operand")]
    public string Operand { get; set; }
}
