using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Api.Authentication;

public class ApiKeyXml : BaseAuthenticationXml
{
    [XmlAttribute("name")]
    [DefaultValue("apiKey")]
    public string Name { get; set; } = "apiKey";

    [XmlText]
    public string Value { get; set; }
}
