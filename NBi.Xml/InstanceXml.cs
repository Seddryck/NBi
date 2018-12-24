using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml
{
    public class InstanceXml
    {
        [XmlElement("variable")]
        public InstanceVariableXml Variable { get; set; }

        private static InstanceXml _unique { get; set; }
        public static InstanceXml Unique
        {
            get
            {
                _unique = _unique ?? new InstanceUnique();
                return _unique;
            }
        }

        internal class InstanceUnique : InstanceXml
        { }
    }
}
