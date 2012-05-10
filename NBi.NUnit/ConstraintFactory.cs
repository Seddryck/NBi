using System;
using NBi.Xml.Constraints;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class ConstraintFactory
    {
        public static Constraint Instantiate(AbstractConstraintXml xml)
        {
            switch (xml.GetType().Name)
            {
                case "EqualToXml": return Instantiate((EqualToXml)xml);
                case "FasterThanXml": return Instantiate((FasterThanXml)xml);
                case "SyntacticallyCorrectXml": return Instantiate((SyntacticallyCorrectXml)xml);
                case "CountXml": return Instantiate((CountXml)xml);
            }
            throw new ArgumentException();
        }
        
        protected static EqualToConstraint Instantiate(EqualToXml xml)
        {
            if (!string.IsNullOrEmpty(xml.ResultSetFile))
                return new EqualToConstraint(xml.ResultSetFile);
            else if (xml.Command != null)
                return new EqualToConstraint(xml.Command, "", "");

            throw new ArgumentException();
        }

        protected static FasterThanConstraint Instantiate(FasterThanXml xml)
        {
            var ctr = new FasterThanConstraint(xml.MaxTimeMilliSeconds, xml.CleanCache);
            return ctr;
        }

        protected static SyntacticallyCorrectConstraint Instantiate(SyntacticallyCorrectXml xml)
        {
            var ctr = new SyntacticallyCorrectConstraint();
            return ctr;
        }

        protected static CountConstraint Instantiate(CountXml xml)
        {
            var ctr = new NBi.NUnit.CountConstraint();
            if (xml.Specification.IsExactlySpecified)
                ctr = ctr.Exactly(xml.Exactly);

            if (xml.Specification.IsMoreThanSpecified)
                ctr = ctr.MoreThan(xml.MoreThan);

            if (xml.Specification.IsLessThanSpecified)
                ctr = ctr.LessThan(xml.LessThan);
            return ctr;
        }
    }
}
