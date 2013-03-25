﻿using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet;
using NBi.NUnit.Member;
using NBi.NUnit.Query;
using NBi.NUnit.Structure;
using NF = NUnit.Framework;

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

        public static EqualToConstraint EqualTo(ResultSet resultSet)
        {
            var ctr = new EqualToConstraint(resultSet);
            return ctr;
        }

        public static EqualToConstraint EqualTo(IDbCommand command)
        {
            var ctr = new EqualToConstraint(command);
            return ctr;
        }

        public static EqualToConstraint EqualTo(IEnumerable<IRow> rows)
        {
            var ctr = new EqualToConstraint(rows);
            return ctr;
        }

        public new static OrderedConstraint Ordered()
        {
            var ctr = new OrderedConstraint();
            return ctr;
        }

        public static SubsetOfConstraint SubsetOf(IEnumerable<string> values)
        {
            var ctr = new SubsetOfConstraint(values);
            return ctr;
        }
    }
}
