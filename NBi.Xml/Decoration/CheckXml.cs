using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Decoration.Check;
using NBi.Xml.Decoration.Command;

namespace NBi.Xml.Decoration
{
    public class CheckXml
    {
        [XmlElement(Type = typeof(ServiceRunningXml), ElementName = "service-running")
        ]
        public List<DecorationCheckXml> Predicates { get; set; }

        public CheckXml()
        {
            Predicates = new List<DecorationCheckXml>();
        }
    }
}
