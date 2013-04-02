using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class FasterThanXml : AbstractConstraintXml
    {
        [XmlAttribute("max-time-milliSeconds")]
        [DefaultValue(false)]
        public int MaxTimeMilliSeconds { get; set; }

        [XmlAttribute("clean-cache")]
        [DefaultValue(false)]
        public bool CleanCache { get; set; }

    }
}
