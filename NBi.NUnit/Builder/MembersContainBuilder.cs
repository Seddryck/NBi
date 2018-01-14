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

        protected NBiConstraint InstantiateConstraint(ContainXml ctrXml)
        {
            Member.ContainConstraint ctr = null;
            if (ctrXml.Query != null)
            {
                var builder = new ResultSetResolverArgsBuilder(ServiceLocator);
                builder.Setup(ctrXml.Query);
                builder.Setup(ctrXml.Settings);
                builder.Build();

                var factory = ServiceLocator.GetResultSetResolverFactory();
                var resolver = factory.Instantiate(builder.GetArgs());
                ctr = new Member.ContainConstraint(resolver);
            }
            else if (ctrXml.Members != null)
            {
                var disco = InstantiateMembersDiscovery(ctrXml.Members);
                ctr = new Member.ContainConstraint(disco);
            }
            else if (ctrXml.GetItems().Count() == 1)
                ctr = new Member.ContainConstraint(ctrXml.Caption);
            else
                ctr = new Member.ContainConstraint(ctrXml.GetItems());

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
