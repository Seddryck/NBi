using System;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints
{
    public class EquivalentToXml : AbstractConstraintForCollectionXml
    {
        [XmlElement("members")]
        public MembersXml Members { get; set; }
        
    }
}
