using System.Xml.Serialization;
using NBi.NUnit;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class FasterThanXml : AbstractConstraintXml
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("maxTimeMilliSeconds")]
        public int MaxTimeMilliSeconds { get; set; }

        public override Constraint Define()
        {
            var ctr = new FasterThanConstraint(ConnectionString, MaxTimeMilliSeconds);
            return ctr;
        }
    }
}
