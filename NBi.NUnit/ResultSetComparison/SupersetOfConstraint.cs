using NBi.Core.ResultSet;
using NBi.Core.Xml;
using NBi.Framework.FailureMessage;
using NBi.NUnit.Query;
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
        
        public SupersetOfConstraint(string value)
            : base(value)
        { }

        public SupersetOfConstraint(ResultSet value)
            : base(value)
        { }

        public SupersetOfConstraint(IContent value)
            : base(value)
        { }

        public SupersetOfConstraint(IDbCommand value)
            : base(value)
        { }

        public SupersetOfConstraint(XPathEngine xpath)
            : base(xpath)
        { }
    }
}
