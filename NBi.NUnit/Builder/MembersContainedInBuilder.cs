using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.Query.Resolver;
using NBi.NUnit.Builder.Helper;
using NBi.Core.ResultSet.Resolver;

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
            Member.ContainedInConstraint ctr;
            if (ctrXml.Query != null)
            {
                var builder = new ResultSetResolverArgsBuilder(ServiceLocator);
                builder.Setup(ctrXml.Query);
                builder.Setup(ctrXml.Settings);
                builder.Build();

                var factory = ServiceLocator.GetResultSetResolverFactory();
                var resolver = factory.Instantiate(builder.GetArgs());
                ctr = new Member.ContainedInConstraint(resolver);
            }
            else if (ctrXml.Members != null)
            {
                var disco = InstantiateMembersDiscovery(ctrXml.Members);
                ctr = new Member.ContainedInConstraint(disco);
            }
            else
                ctr = new Member.ContainedInConstraint(ctrXml.GetItems());

            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

    }
}
