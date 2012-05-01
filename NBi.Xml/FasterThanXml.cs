using System.Xml.Serialization;

namespace NBi.Xml
{
    public class FasterThanXml : AbstractConstraintXml
    {
        [XmlAttribute("maxTimeMilliSeconds")]
        public int MaxTimeMilliSeconds { get; set; }

        [XmlAttribute("cleanCache")]
        public bool CleanCache { get; set; }

    }
}
