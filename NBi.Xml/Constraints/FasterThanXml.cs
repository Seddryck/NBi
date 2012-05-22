using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class FasterThanXml : AbstractConstraintXml
    {
        [XmlAttribute("max-time-milliSeconds")]
        public int MaxTimeMilliSeconds { get; set; }

        [XmlAttribute("clean-cache")]
        public bool CleanCache { get; set; }

    }
}
