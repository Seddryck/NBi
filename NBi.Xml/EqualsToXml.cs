using System.Xml.Serialization;
using NBi.NUnit;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class EqualsToXml : AbstractConstraintXml
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("resultSetPath")]
        public string ResultSetPath { get; set; }

        public override Constraint Define()
        {
            var ctr = new EqualToConstraint(ConnectionString, ResultSetPath);
            return ctr;
        }
    }
}
