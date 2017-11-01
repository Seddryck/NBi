using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet.Loading;
using NBi.NUnit.Member;
using NBi.NUnit.Query;
using NBi.NUnit.ResultSetComparison;
using NBi.NUnit.Structure;
using NF = NUnit.Framework;
using NBi.Core.ResultSet;

namespace NBi.NUnit.FluentInterface
{
    public class Is : NF.Is
    {

        public Is()
        {
        }

        public static SyntacticallyCorrectConstraint SyntacticallyCorrect()
        {
            return new SyntacticallyCorrectConstraint();
        }

        public static FasterThanConstraint FasterThan(int maxTimeMilliSeconds)
        {
            var ctr = new FasterThanConstraint();
            ctr.MaxTimeMilliSeconds(maxTimeMilliSeconds);
            return ctr;
        }

        public static EqualToConstraint EqualTo(IResultSetService service)
        {
            var ctr = new EqualToConstraint(service);
            return ctr;
        }
        
        public new static OrderedConstraint Ordered()
        {
            var ctr = new OrderedConstraint();
            return ctr;
        }

        public static NBi.NUnit.Structure.ContainedInConstraint SubsetOf(IEnumerable<string> values)
        {
            var ctr = new NBi.NUnit.Structure.ContainedInConstraint(values);
            return ctr;
        }
    }
}
