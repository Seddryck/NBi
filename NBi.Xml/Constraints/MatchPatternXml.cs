using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;

namespace NBi.Xml.Constraints
{
    public class MatchPatternXml : AbstractConstraintXml
    {

        [XmlElement("regex")]
        public string Regex { get; set; }

        public MatchPatternXml()
        {
            
        }

    }
}
