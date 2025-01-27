using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Members.Ranges;

namespace NBi.Xml.Items.Ranges;

public class IntegerRangeXml : RangeXml, IIntegerRange
{
    [XmlAttribute("start")]
    public int Start { get; set; }
    [XmlAttribute("end")]
    public int End { get; set; }
    [XmlAttribute("step")]
    [DefaultValue(1)]
    public int Step { get; set; }

    public IntegerRangeXml()
    {
        Step = 1;
    }
}