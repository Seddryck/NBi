using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Query
{
    public class NoRowsConstraint : AllRowsConstraint
    {
        public NoRowsConstraint(IResultSetFilter filter)
            :base(filter)
        {
            filterFunction = filter.Apply;
        }
       
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("no rows validate the predicate");
        }

        public override void WriteFilterMessageTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteLine("Rows validating the predicate:");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            var value = filterResultSet.Rows.Count;
            writer.WriteLine("{0} row{1} validate{2} the predicate", value, value > 1 ? "s" : string.Empty, value == 1 ? "s" : string.Empty);
        }
    }
}