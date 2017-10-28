using NBi.Core.ResultSet.Loading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetComparison
{
    class SupersetOfConstraint : BaseResultSetComparisonConstraint
    {
        public SupersetOfConstraint(IResultSetLoader service)
            : base(service)
        { }
    }
}
