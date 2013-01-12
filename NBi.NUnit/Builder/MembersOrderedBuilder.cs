using System;
using System.Linq;
using NBi.Core.Analysis.Request;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

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

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(OrderedXml ctrXml)
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
                    ctr = ctr.Specific(ctrXml.Definition);
                    break;
                default:
                    break;
            }

            return ctr;
        }

        

    }
}
