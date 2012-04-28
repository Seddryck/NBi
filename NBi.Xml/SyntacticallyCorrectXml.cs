using System.Xml.Serialization;
using NBi.NUnit;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class SyntacticallyCorrectXml : AbstractConstraintXml
    {
        public override Constraint Define()
        {
            var ctr = new SyntacticallyCorrectConstraint();
            return ctr;
        }
    }
}
