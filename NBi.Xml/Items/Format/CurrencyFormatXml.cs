using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Format;

namespace NBi.Xml.Items.Format
{
    public class CurrencyFormatXml : NumericFormatXml, ICurrencyFormat
    {
        [XmlAttribute("currency-symbol")]
        public string CurrencySymbol { get; set; }

        [XmlAttribute("currency-pattern")]
        public CurrencyPattern CurrencyPattern { get; set; }

        public CurrencyFormatXml() : base()
        {
        }
    }
}
