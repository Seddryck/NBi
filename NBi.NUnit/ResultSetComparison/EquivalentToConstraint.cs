using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;


namespace NBi.NUnit.ResultSetComparison
{
    public class EquivalentToConstraint : BaseResultSetComparisonConstraint
    {
        public EquivalentToConstraint(IResultSetService service)
            : base(service)
        { }
    }
}
