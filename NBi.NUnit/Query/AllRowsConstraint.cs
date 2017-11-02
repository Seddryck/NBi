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
    public class AllRowsConstraint : RowCountFilterConstraint
    {
        public AllRowsConstraint(IResultSetFilter filter)
            :base(null, filter)
        {
            filterFunction = filter.AntiApply;
        }
        protected override bool doMatch(int actual)
        {
            return filterResultSet.Rows.Count == 0;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            else
                writer.WritePredicate($"all rows validate the predicate '{filter.Describe()}'.");
        }

        public override void WriteFilterMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WriteLine("Rows not validating the predicate:");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            var value = filterResultSet.Rows.Count;
            writer.WriteLine($"{value} row{(value > 1 ? "s" : string.Empty)} do{(value == 1 ? "es" : string.Empty)}n't validate the predicate '{filter.Describe()}'.");
        }
    }
}