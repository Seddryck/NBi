using System.Xml.Serialization;
using NBi.NUnit;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class SyntacticallyCorrectXml : AbstractConstraintXml
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override Constraint Define()
        {
            var ctr = new SyntacticallyCorrectConstraint(ConnectionString);
            return ctr;
        }
    }
}
