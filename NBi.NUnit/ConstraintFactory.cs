using System;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;
using NBi.Xml.Constraints;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class ConstraintFactory
    {
        public static Constraint Instantiate(AbstractConstraintXml xml, Type systemType)
        {
            if (xml.GetType() == typeof(EqualToXml)) return Instantiate((EqualToXml)xml);
            if (xml.GetType() == typeof(FasterThanXml)) return Instantiate((FasterThanXml)xml);
            if (xml.GetType() == typeof(SyntacticallyCorrectXml)) return Instantiate((SyntacticallyCorrectXml)xml);
            if (xml.GetType() == typeof(CountXml)) return Instantiate((CountXml)xml);
            if (xml.GetType() == typeof(ContainsXml)) return Instantiate((ContainsXml)xml, systemType);

            throw new ArgumentException(string.Format("{0} is not an expected type.",xml.GetType().Name));
        }
        
        protected static EqualToConstraint Instantiate(EqualToXml xml)
        {
            if (!string.IsNullOrEmpty(xml.ResultSetFile))
            {
                var ctr = new EqualToConstraint(xml.ResultSetFile);
                return ctr;
            }
            else if (xml.Command != null)
            {
                var ctr = new EqualToConstraint(xml.Command);
                return ctr;
            }

            throw new ArgumentException();
        }

        protected static FasterThanConstraint Instantiate(FasterThanXml xml)
        {
            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(xml.MaxTimeMilliSeconds);
            if (xml.CleanCache)
                ctr = ctr.CleanCache();
            return ctr;
        }

        protected static SyntacticallyCorrectConstraint Instantiate(SyntacticallyCorrectXml xml)
        {
            var ctr = new SyntacticallyCorrectConstraint();
            return ctr;
        }

        protected static NBi.NUnit.Member.CountConstraint Instantiate(CountXml xml)
        {
            var ctr = new NBi.NUnit.Member.CountConstraint();
            if (xml.Specification.IsExactlySpecified)
                ctr = ctr.Exactly(xml.Exactly);

            if (xml.Specification.IsMoreThanSpecified)
                ctr = ctr.MoreThan(xml.MoreThan);

            if (xml.Specification.IsLessThanSpecified)
                ctr = ctr.LessThan(xml.LessThan);
            return ctr;
        }

        protected static Constraint Instantiate(ContainsXml xml, Type systemType)
        {

            if (systemType == typeof(MetadataQuery))
                return InstantiateForStructure(xml);
            if (systemType == typeof(AdomdMemberCommand))
                return InstantiateForMember(xml);

            throw new ArgumentException(string.Format("{0} is not an expected type.", systemType.Name));
        }

        private static NBi.NUnit.Structure.ContainsConstraint InstantiateForStructure(ContainsXml xml)
        {
            var ctr = new NBi.NUnit.Structure.ContainsConstraint(xml.Caption);

            if (xml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }

        private static NBi.NUnit.Member.ContainsConstraint InstantiateForMember(ContainsXml xml)
        {
            var ctr = new NBi.NUnit.Member.ContainsConstraint(xml.Caption);

            if (xml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            return ctr;
        }
    }
}
