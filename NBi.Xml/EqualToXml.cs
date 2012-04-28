using System.Xml.Serialization;
using NBi.NUnit;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class EqualToXml : AbstractConstraintXml
    {
        [XmlAttribute("resultSet-Path")]
        public string ResultSetPath { get; set; }

        public override Constraint Define()
        {
            var ctr = new EqualToConstraint(ResultSetPath);
            return ctr;
        }
    }
}
