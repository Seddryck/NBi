using System.Xml.Serialization;
using NBi.Xml.Items.Format;

namespace NBi.Xml.Settings
{
    public class ReferenceXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("connectionString")]
        public string ConnectionString { get; set; }

        [XmlElement("regex")]
        public string Regex { get; set; }

        [XmlElement("numeric-format")]
        public NumericFormatXml NumericFormat { get; set; }

        [XmlElement("currency-format")]
        public CurrencyFormatXml CurrencyFormat { get; set; }

    }
}
