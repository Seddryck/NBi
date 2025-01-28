using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Conversion;

public class ConvertXml : AlterationXml
{
    [XmlAttribute("column")]
    public string Column { get; set; }

       [XmlElement(Type = typeof(TextToDateTimeConverterXml), ElementName = "text-to-dateTime")]
    [XmlElement(Type = typeof(TextToDateConverterXml), ElementName = "text-to-date")]
    [XmlElement(Type = typeof(TextToNumericConverterXml), ElementName = "text-to-numeric")]
    public AbstractConverterXml Converter { get; set; }

}
