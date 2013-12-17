using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersContainBuilder : AbstractMembersBuilder
    {
        protected ContainXml ConstraintXml {get; set;}

        public MembersContainBuilder() : base()
        {
        }

        internal MembersContainBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ContainXml))
                throw new ArgumentException("Constraint must be a 'ContainsXml'");

            ConstraintXml = (ContainXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(ContainXml ctrXml)
        {
            NBi.NUnit.Member.ContainConstraint ctr = null;
            if (ctrXml.Query != null)
                ctr = new NBi.NUnit.Member.ContainConstraint(ctrXml.Query.GetCommand());
            else if (ctrXml.GetItems().Count() == 1)
                ctr = new NBi.NUnit.Member.ContainConstraint(ctrXml.Caption);
            else
                ctr = new NBi.NUnit.Member.ContainConstraint(ctrXml.GetItems());

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
