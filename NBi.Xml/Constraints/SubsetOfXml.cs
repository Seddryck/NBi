using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints
{
    public class SubsetOfXml : AbstractConstraintForCollectionXml
    {

        [XmlElement("members")]
        public MembersXml Members { get; set; }
    }
}
