using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.ResultSetComparison
{
    public class EqualToConstraint : BaseResultSetComparisonConstraint
    {
        public EqualToConstraint(IResultSetResolver resolver)
            : base(resolver)
        { }
    }
}
