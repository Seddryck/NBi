using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class FileXml
    {
        [Obsolete("Use 'Path' instead of 'Value'")]
        public string Value { get => Path; set => Path = value; }

        [XmlElement("path")]
        public string Path { get; set; }
    }
}
