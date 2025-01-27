using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Api.Authentication;

public class NtmlUserPasswordXml : BaseAuthenticationXml
{
    [XmlAttribute("username")]
    public string Username { get; set; }

    [XmlAttribute("password")]
    public string Password { get; set; }
}
