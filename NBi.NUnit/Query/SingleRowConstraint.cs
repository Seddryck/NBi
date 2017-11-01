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
    public class SingleRowConstraint : NoRowsConstraint
    {
        public SingleRowConstraint(IResultSetFilter filter)
            : base(filter)
        { }

        protected override bool doMatch(int actual)
        {
            return filterResultSet.Rows.Count == 1;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WritePredicate($"single row validates the predicate '{filter.Describe()}'.");
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
            if (filterResultSet.Rows.Count == 0)
                writer.WriteLine($"No row validates the predicate '{filter.Describe()}'.");
            else
                writer.WriteLine($"{filterResultSet.Rows.Count} rows validate the predicate '{filter.Describe()}'.");
        }
    }
}