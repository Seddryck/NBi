using System;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.ResultSetComparison;
using NBi.Core.ResultSet.Equivalence;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.Builder
{
    class ResultSetSubsetOfBuilder : ResultSetEqualToBuilder
    {
        protected override EquivalenceKind EquivalenceKind
        { get => EquivalenceKind.SubsetOf; }

        public ResultSetSubsetOfBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SubsetOfXml))
                throw new ArgumentException("Constraint must be a 'SubsetOfXml'");

            ConstraintXml = (SubsetOfXml)ctrXml;
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IResultSetResolver resolver)
            => new SubsetOfConstraint(resolver);
    }
}
