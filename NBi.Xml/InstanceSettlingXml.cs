using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml
{
    public class InstanceSettlingXml
    {
        [XmlElement("local-variable")]
        public InstanceVariableXml Variable { get; set; }

        private static InstanceSettlingXml _unique { get; set; }
        public static InstanceSettlingXml Unique
        {
            get
            {
                _unique = _unique ?? new InstanceUnique();
                return _unique;
            }
        }

        internal class InstanceUnique : InstanceSettlingXml
        { }
    }
}
