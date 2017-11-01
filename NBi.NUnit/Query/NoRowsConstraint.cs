using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;

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
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WritePredicate($"no rows validate the predicate '{filter.Describe()}'.");
        }

        public override void WriteFilterMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WriteLine("Rows validating the predicate:");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            var value = filterResultSet.Rows.Count;
            writer.WriteLine($"{value} row{(value > 1 ? "s" : string.Empty)} validate{(value == 1 ? "s" : string.Empty)} the predicate '{filter.Describe()}'.");
        }
    }
}