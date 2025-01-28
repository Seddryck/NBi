using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Api.Authentication;

public class OAuth2Xml : BaseAuthenticationXml
{
    [XmlElement("access-token")]
    public string AccessToken { get; set; }

    [XmlAttribute("token-type")]
    public string TokenType { get; set; }
}
