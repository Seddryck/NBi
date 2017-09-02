using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class MembersContainedInBuilder : AbstractMembersBuilder
    {
        protected ContainedInXml ConstraintXml { get; set; }

        public MembersContainedInBuilder() : base()
        {
        }

        internal MembersContainedInBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ContainedInXml))
                throw new ArgumentException("Constraint must be a 'MembersSubsetOfBuilder'");

            ConstraintXml = (ContainedInXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(ContainedInXml ctrXml)
        {
            NBi.NUnit.Member.ContainedInConstraint ctr;
            if (ctrXml.Query != null)
                ctr = new NBi.NUnit.Member.ContainedInConstraint(ctrXml.Query.GetCommand());
            else if (ctrXml.Members != null)
            {
                var disco = InstantiateMembersDiscovery(ctrXml.Members);
                ctr = new NBi.NUnit.Member.ContainedInConstraint(disco);
            }
            else
                ctr = new NBi.NUnit.Member.ContainedInConstraint(ctrXml.GetItems());

            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

    }
}
