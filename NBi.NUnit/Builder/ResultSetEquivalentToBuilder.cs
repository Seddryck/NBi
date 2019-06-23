using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.ResultSetComparison;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class ResultSetEquivalentToBuilder : ResultSetEqualToBuilder
    {
        protected override EquivalenceKind EquivalenceKind  { get => EquivalenceKind.EquivalentTo; }

        public ResultSetEquivalentToBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ResultSetEquivalentToXml))
                throw new ArgumentException("Constraint must be a 'ResultSetEquivalentToXml'");

            ConstraintXml = (ResultSetEquivalentToXml)ctrXml;
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(IResultSetService service)
            => new EquivalentToConstraint(service);

    }
}
