using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Scalar.Comparer;

public class Rounding
{
    public enum RoundingStyle
    {
        [XmlEnum(Name = "none")]
        None = 0,
        [XmlEnum(Name = "floor")]
        Floor,
        [XmlEnum(Name = "ceiling")]
        Ceiling,
        [XmlEnum(Name = "round")]
        Round
    }

    public string Step { get; private set; }
    public RoundingStyle Style { get; private set; }

    protected Rounding(string step, RoundingStyle style)
    {
        Step = step;
        Style = style; 
    }

    protected decimal GetValue(decimal value, decimal step)
    {
        var remainder = Math.Abs(value) % step;

        if ((Style == RoundingStyle.Ceiling && value > 0 && remainder!=0)
            || (Style == RoundingStyle.Floor && value < 0 && remainder != 0)
            || (Style == RoundingStyle.Round && remainder >= step / 2))
            remainder -= step;

        return value - (remainder * Math.Sign(value));
    }
}
