using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersCountBuilder : AbstractMembersBuilder
    {
        protected CountXml ConstraintXml {get; set;}

        public MembersCountBuilder() : base()
        {
        }

        internal MembersCountBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is CountXml))
                throw new ArgumentException("Constraint must be a 'CountXml'");

            ConstraintXml = (CountXml)ctrXml;
        }

        protected NBiConstraint InstantiateConstraint(CountXml ctrXml)
        {
            var ctr = new NBi.NUnit.Member.CountConstraint();
            if (ctrXml.ExactlySpecified)
                ctr = ctr.Exactly(ctrXml.Exactly);

            if (ctrXml.MoreThanSpecified)
                ctr = ctr.MoreThan(ctrXml.MoreThan);

            if (ctrXml.LessThanSpecified)
                ctr = ctr.LessThan(ctrXml.LessThan);
            return ctr;
        }

    }
}
