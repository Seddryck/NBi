using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items.Api.Rest;

public class RestPathXml
{
    [XmlText]
    public string Value { get; set; }
}
