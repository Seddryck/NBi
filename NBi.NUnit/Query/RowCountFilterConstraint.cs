using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;

namespace NBi.NUnit.Query
{
    public class RowCountFilterConstraint : RowCountConstraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IResultSetFilter filter = ResultSetFilter.None;
        protected IResultSet filterResultSet;
        protected Func<IResultSet, IResultSet> filterFunction;

        public RowCountFilterConstraint(DifferedConstraint childConstraint, IResultSetFilter filter)
            : base(childConstraint)
        {
            this.filter = filter;
            filterFunction = filter.Apply;
        }

        protected override IDataRowsMessageFormatter BuildFailure()
        {
            var factory = new DataRowsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, EngineStyle.ByIndex);
            msg.BuildFilter(actualResultSet.Rows, filterResultSet.Rows);
            return msg;
        }
        
        protected override bool doMatch(IResultSet actual)
        {
            actualResultSet = actual;
            filterResultSet = filterFunction(actualResultSet);
            return Matches(filterResultSet.Rows.Count);
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WritePredicate($"count of rows validating the predicate '{filter.Describe()}' is");
            ctr.WriteDescriptionTo(writer);
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(Failure.RenderMessage());
            else
            {
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine();
                WriteFilterMessageTo(writer);
                writer.WriteLine(Failure.RenderAnalysis());
            }
        }

        public virtual void WriteFilterMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WriteLine("Filtered version of the result-set:");
        }

    }
}