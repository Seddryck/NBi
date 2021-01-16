using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NUnit.Framework;
using NBi.Extensibility;
using NBi.Core.Configuration.FailureReport;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.Query
{
    public class RowCountConstraint : NBiConstraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IResultSet actualResultSet;
        protected DifferedConstraint differed;
        protected NUnitCtr.Constraint ctr;

        public RowCountConstraint(DifferedConstraint childConstraint)
        {
            differed = childConstraint;
        }

        internal DifferedConstraint Differed
        {
            get => differed;
        }

        private IDataRowsMessageFormatter failure;
        protected IDataRowsMessageFormatter Failure { get => failure ?? (failure = BuildFailure());}

        protected virtual IDataRowsMessageFormatter BuildFailure()
        {
            var factory = new DataRowsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, EngineStyle.ByIndex);
            msg.BuildCount(actualResultSet.Rows.Cast<DataRow>());
            return msg;
        }

        /// <summary>
        /// Handle an IDbCommand and compare its row-count to a another value
        /// </summary>
        /// <param name="actual">An IResultSetService or ResultSet</param>
        /// <returns>true, if the row-count of ResultSet validates the child constraint</returns>
        public override bool Matches(object actual)
        {
            if (actual is IResultSetResolver)
                return Matches(((IResultSetResolver)actual).Execute());
            else if (actual is ResultSet)
                return doMatch(actual as ResultSet);
            else if (actual is int)
            {
                var output = doMatch(((int)actual));

                if (output && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
                    Assert.Pass(Failure.RenderMessage());

                return output;
            }
                
            else
                throw new ArgumentException($"The type '{actual.GetType().Name}' is not supported by the constraint '{this.GetType().Name}'. Use a IResultSetService or a ResultSet.", nameof(actual));
        }

        protected virtual bool doMatch(IResultSet actual)
        {
            actualResultSet = actual;
            return Matches(actualResultSet.Rows.Count);
        }

        protected virtual bool doMatch(int actual)
        {
            this.actual = actual;
            ctr = differed.Resolve();
            var output = ctr.Matches(actual);
            return output;
        }
       
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("count of rows returned by the query is");
            ctr.WriteDescriptionTo(writer);
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            base.WriteMessageTo(writer);
            writer.WriteLine();
            writer.WriteLine("Actual result-set returned by the query:");
            writer.WriteLine(Failure.RenderActual());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            ctr.WriteActualValueTo(writer);
        }
    }
}