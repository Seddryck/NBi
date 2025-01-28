using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Members.Ranges;

namespace NBi.Xml.Items.Ranges;

public class DateRangeXml : RangeXml, IDateRange
{
    [XmlAttribute("start")]
    public DateTime Start { get; set; }
    [XmlAttribute("end")]
    public DateTime End { get; set; }
    [XmlAttribute("culture")]
    public string CultureSymbol { get; set; }
    [XmlAttribute("format")]
    public string Format { get; set; }

    [XmlIgnore]
    public CultureInfo Culture
    {
        get
        {
            return new CultureInfo(CultureSymbol);
        }
        set
        {
            CultureSymbol = value.Name;
        }
    }
}