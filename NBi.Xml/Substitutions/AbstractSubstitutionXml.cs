using NBi.Xml.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Substitutions
{
    public class AbstractSubstitutionXml
    {
        [XmlElement("database-object")]
        public DatabaseObjectXml DatabaseObject { get; set; }
    }
}
