using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;


namespace NBi.NUnit.ResultSetComparison
{
    public class IntersectionOfConstraint : BaseResultSetComparisonConstraint
    {
        public IntersectionOfConstraint(IResultSetService service)
            : base(service)
        { }
    }
}
