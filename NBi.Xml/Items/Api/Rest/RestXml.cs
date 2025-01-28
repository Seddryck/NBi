using NBi.Xml.Items.Api.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Api.Rest;

public class RestXml
{
    [XmlAttribute("base-address")]
    public string BaseAddress { get; set; }

    [XmlElement("authentication")]
    public AuthenticationXml Authentication { get; set; } = new AuthenticationXml { Protocol = new AnonymousXml() };

    public bool ShouldSerializeAuthentication()
        => !(Authentication?.Protocol is AnonymousXml);

    [XmlElement("header")]
    public List<RestHeaderXml> Headers { get; set; } = new List<RestHeaderXml>();

    [XmlElement("path")]
    public RestPathXml Path { get; set; }

    [XmlElement("segment")]
    public List<RestSegmentXml> Segments { get; set; } = new List<RestSegmentXml>();

    [XmlElement("parameter")]
    public List<RestParameterXml> Parameters { get; set; } = new List<RestParameterXml>();


}
