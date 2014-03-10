using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Decoration.Condition;

namespace NBi.Xml.Decoration
{
    public class ConditionXml
    {
        [XmlElement(Type = typeof(ServiceRunningXml), ElementName = "service-running")
        ]
        public List<DecorationConditionXml> Predicates { get; set; }

        public ConditionXml()
        {
            Predicates = new List<DecorationConditionXml>();
        }
    }
}
