using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Members.Ranges;

namespace NBi.Xml.Items.Ranges;

public class PatternIntegerRangeXml : IntegerRangeXml, IPatternDecorator
{
    [XmlAttribute("position")]
    public PositionValue Position { get; set; }
    [XmlAttribute("pattern")]
    public string Pattern { get; set; }

    public PatternIntegerRangeXml() 
        : base()
    {}
}