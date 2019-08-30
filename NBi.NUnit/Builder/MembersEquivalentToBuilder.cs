using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet.Resolver;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class MembersEquivalentToBuilder : AbstractMembersBuilder
    {
        protected EquivalentToXml ConstraintXml { get; set; }

        public MembersEquivalentToBuilder() : base()
        {
        }

        internal MembersEquivalentToBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is EquivalentToXml))
                throw new ArgumentException("Constraint must be a 'EquivalentToXml'");

            ConstraintXml = (EquivalentToXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(EquivalentToXml ctrXml)
        {
            Member.EquivalentToConstraint ctr;
            if (ctrXml.Query != null)
            {
                var builder = new ResultSetResolverArgsBuilder(ServiceLocator);
                builder.Setup(ctrXml.Query, ctrXml.Settings, SettingsXml.DefaultScope.Assert, Variables);
                builder.Build();

                var factory = ServiceLocator.GetResultSetResolverFactory();
                var resolver = factory.Instantiate(builder.GetArgs());
                ctr = new Member.EquivalentToConstraint(resolver);
            }
            else if (ctrXml.Members != null)
            {
                var disco = InstantiateMembersDiscovery(ctrXml.Members);
                ctr = new Member.EquivalentToConstraint(disco);
            }
            else
                ctr = new Member.EquivalentToConstraint(ctrXml.GetItems());

            //Ignore-case if requested
            if (ctrXml.IgnoreCase)
                ctr = ctr.IgnoreCase;
            return ctr;
        }

    }
}
