using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Items
{
    public class DimensionXml : AbstractMembersItem
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        public override object Instantiate()
        {
            //TODO Here?
            return null;
        }

        [XmlIgnore]
        public string Path { get { return string.Format("[{0}]", Caption); } }
    }
}
