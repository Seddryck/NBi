using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Core.Analysis.Member;

namespace NBi.Model.Xml.Items;

public class PatternXml
{
    [XmlAttribute("pattern")]
    public Pattern Pattern { get; set; }

    [XmlText]
    public string Value { get; set; }
}
