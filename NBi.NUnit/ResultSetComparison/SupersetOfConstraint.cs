using NBi.Core.ResultSet.Equivalence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.ResultSetComparison
{
    class SupersetOfConstraint : BaseResultSetComparisonConstraint
    {
        protected override EquivalenceKind ComparisonKind => EquivalenceKind.SupersetOf;

        public SupersetOfConstraint(IResultSetResolver service)
            : base(service)
        { }
    }
}
