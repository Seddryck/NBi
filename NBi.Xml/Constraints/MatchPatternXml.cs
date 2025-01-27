using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items.Format;
using NBi.Xml.Settings;

namespace NBi.Xml.Constraints;

public class MatchPatternXml : AbstractConstraintXml, IReferenceFriendly
{
    [XmlElement("regex")]
    public string Regex { get; set; }

    [XmlElement("numeric-format")]
    public NumericFormatXml NumericFormat { get; set; }

    [XmlElement("currency-format")]
    public CurrencyFormatXml CurrencyFormat { get; set; }

    [XmlIgnore]
    public bool NumericFormatSpecified
    {
        get
        {
            return !NumericFormat.IsEmpty;
        }
    }
    [XmlIgnore]
    public bool CurrencyFormatSpecified
    {
        get
        {
            return !CurrencyFormat.IsEmpty;
        }
    }

    public MatchPatternXml()
    {
        NumericFormat = new NumericFormatXml(true);
        CurrencyFormat = new CurrencyFormatXml(true);
    }

    public void AssignReferences(IEnumerable<ReferenceXml> references)
    {
        NumericFormat.AssignReferences(references);
        CurrencyFormat.AssignReferences(references);
    }
}
