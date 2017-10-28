using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet.Loading;

namespace NBi.NUnit.ResultSetComparison
{
    public class EqualToConstraint : BaseResultSetComparisonConstraint
    {

        public EqualToConstraint(IResultSetLoader value)
            : base(value)
        { }
    }
}
