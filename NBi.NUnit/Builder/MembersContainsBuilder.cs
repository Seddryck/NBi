using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersContainsBuilder : AbstractMembersBuilder
    {
        protected ContainsXml ConstraintXml {get; set;}

        public MembersContainsBuilder() : base()
        {
        }

        internal MembersContainsBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ContainsXml))
                throw new ArgumentException("Constraint must be a 'ContainsXml'");

            ConstraintXml = (ContainsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(ContainsXml ctrXml)
        {
            NBi.NUnit.Member.ContainsConstraint ctr = null;

            if (ctrXml.Items.Count == 1)
                ctr = new NBi.NUnit.Member.ContainsConstraint(ctrXml.Caption);
            else
                ctr = new NBi.NUnit.Member.ContainsConstraint(ctrXml.Items);

            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;

            //Exactly
            //if (ctrXml.Exactly)
                //ctr = ctr.Exactly;

            return ctr;
        }

    }
}
