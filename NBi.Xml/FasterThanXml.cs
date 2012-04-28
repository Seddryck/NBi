using System.Xml.Serialization;
using NBi.NUnit;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class FasterThanXml : AbstractConstraintXml
    {
        [XmlAttribute("maxTimeMilliSeconds")]
        public int MaxTimeMilliSeconds { get; set; }

        [XmlAttribute("cleanCache")]
        public bool CleanCache { get; set; }

        public override Constraint Define()
        {
            var ctr = new FasterThanConstraint(MaxTimeMilliSeconds, CleanCache);
            return ctr;
        }
    }
}
