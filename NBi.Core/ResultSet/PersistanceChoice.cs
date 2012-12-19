using System.Xml.Serialization;

namespace NBi.Core.ResultSet
{
    public enum PersistanceChoice
    {
        [XmlEnum(Name = "never")]
        Never = 0,
        [XmlEnum(Name = "only-if-failed")]
        OnlyIfFailed = 1,
        [XmlEnum(Name = "always")]
        Always = 2
    }
}
