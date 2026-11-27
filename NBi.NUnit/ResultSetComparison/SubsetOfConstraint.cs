using NBi.Core.ResultSet.Equivalence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.ResultSetComparison
{
    public class SubsetOfConstraint : BaseResultSetComparisonConstraint
    {
        protected override EquivalenceKind ComparisonKind => EquivalenceKind.SubsetOf;

        public SubsetOfConstraint(IResultSetResolver service)
            : base(service)
        { }
    }
}
