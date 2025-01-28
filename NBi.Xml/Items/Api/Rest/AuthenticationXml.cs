using NBi.Xml.Items.Api.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Api.Rest;

public class AuthenticationXml
{
    [XmlElement(Type = typeof(AnonymousXml), ElementName = "anonymous"),
    XmlElement(Type = typeof(ApiKeyXml), ElementName = "api-key"),
    XmlElement(Type = typeof(HttpBasicXml), ElementName = "http-basic"),
    XmlElement(Type = typeof(NtmlCurrentUserXml), ElementName = "ntml-current-user"),
    XmlElement(Type = typeof(NtmlUserPasswordXml), ElementName = "ntml"),
    XmlElement(Type = typeof(OAuth2Xml), ElementName = "oauth2"),
    ]
    public BaseAuthenticationXml Protocol { get; set; }
}
