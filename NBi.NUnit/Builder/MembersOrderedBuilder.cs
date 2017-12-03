using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Xml.Items;
using NBi.Core.Query;

namespace NBi.NUnit.Builder
{
    class MembersOrderedBuilder : AbstractMembersBuilder
    {
        protected OrderedXml ConstraintXml {get; set;}

        public MembersOrderedBuilder() : base()
        {
        }

        internal MembersOrderedBuilder(DiscoveryRequestFactory factory)
            : base(factory)
        {
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is OrderedXml))
                throw new ArgumentException("Constraint must be a 'OrderedXml'");

            ConstraintXml = (OrderedXml)ctrXml;
        }

        protected NBiConstraint InstantiateConstraint(OrderedXml ctrXml)
        {
            var ctr = new NBi.NUnit.Member.OrderedConstraint();
            if (ctrXml.Descending)
                ctr = ctr.Descending;

            switch (ctrXml.Rule)
            {
                case OrderedXml.Order.Alphabetical:
                    ctr = ctr.Alphabetical;
                    break;
                case OrderedXml.Order.Chronological:
                    ctr = ctr.Chronological;
                    break;
                case OrderedXml.Order.Numerical:
                    ctr = ctr.Numerical;
                    break;
                case OrderedXml.Order.Specific:
                    if (ctrXml.Query != null)
                        ctr = ctr.Specific(BuildQuery(ctrXml.Query));
                    else
                        ctr = ctr.Specific(ctrXml.Definition);
                    break;
                default:
                    break;
            }

            return ctr;
        }

        private IQuery BuildQuery(QueryXml queryXml)
        {
            return new NBi.Core.Query.Query(queryXml.GetQuery(), queryXml.GetConnectionString(), new TimeSpan(0, 0, 0));
        }



    }
}
