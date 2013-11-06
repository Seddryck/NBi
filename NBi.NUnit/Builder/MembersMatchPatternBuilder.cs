using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersMatchPatternBuilder : AbstractMembersBuilder
    {
        protected MatchPatternXml ConstraintXml {get; set;}

        public MembersMatchPatternBuilder() : base()
        {
        }

        internal MembersMatchPatternBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is MatchPatternXml))
                throw new ArgumentException("Constraint must be a 'MatchPatternXml'");

            ConstraintXml = (MatchPatternXml)ctrXml;
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(MatchPatternXml ctrXml)
        {
            var ctr = new NBi.NUnit.Member.MatchPatternConstraint();
            if (!string.IsNullOrEmpty(ctrXml.Regex))
                ctr = ctr.Regex(ctrXml.Regex);

            return ctr;
        }

    }
}
