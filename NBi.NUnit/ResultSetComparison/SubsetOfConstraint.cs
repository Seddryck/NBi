using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Framework;
using NBi.Core.Xml;

namespace NBi.NUnit.ResultSetComparison
{
    public class SubsetOfConstraint : BaseResultSetComparisonConstraint
    {
        public SubsetOfConstraint(string value)
            : base(value)
        {}

        public SubsetOfConstraint(ResultSet value)
            : base(value)
        { }

        public SubsetOfConstraint(IContent value)
            : base(value)
        { }

        public SubsetOfConstraint(IDbCommand value)
            : base(value)
        { }

        public SubsetOfConstraint(XPathEngine xpath)
            : base(xpath)
        { }

    }
}
