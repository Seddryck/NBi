using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    abstract public class AbstractItem
    {
        [XmlAttribute("caption")]
        public string Caption { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public abstract object Instantiate();
    }
}
