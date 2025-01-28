using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints;

public class FasterThanXml : AbstractConstraintXml
{
    [XmlAttribute("max-time-milliSeconds")]
    public int MaxTimeMilliSeconds { get; set; }

    [XmlAttribute("clean-cache")]
    [DefaultValue(false)]
    public bool CleanCache { get; set; }

    [XmlAttribute("timeout-milliSeconds")]
    [DefaultValue(0)]
    public int TimeOutMilliSeconds { get; set; }
}

public class FasterThanOldXml : FasterThanXml { }
