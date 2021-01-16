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
        public SupersetOfConstraint(IResultSetResolver service)
            : base(service)
        { }
    }
}
