using System;
using System.Xml.Serialization;
using NBi.Xml.Items.Format;

namespace NBi.Xml.Constraints
{
    public class MatchPatternXml : AbstractConstraintXml
    {

        [XmlElement("regex")]
        public string Regex { get; set; }

        [XmlElement("numeric-format")]
        public NumericFormatXml NumericFormat { get; set; }

        [XmlElement("currency-format")]
        public CurrencyFormatXml CurrencyFormat { get; set; }

        public MatchPatternXml()
        {
            
        }


        
    }
}
