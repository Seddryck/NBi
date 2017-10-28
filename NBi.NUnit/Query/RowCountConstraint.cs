using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Loading;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Query
{
    public class RowCountConstraint : NBiConstraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected ResultSet actualResultSet;
        protected NUnitCtr.Constraint child;

        public RowCountConstraint(NUnitCtr.Constraint childConstraint)
        {
            child = childConstraint;
        }

        public NUnitCtr.Constraint Child
        {
            get
            {
                return child;
            }
        }

        private DataRowsMessage failure;
        protected DataRowsMessage Failure
        {
            get
            {
                if (failure == null)
                    failure = BuildFailure();
                return failure;
            }
        }

        protected virtual DataRowsMessage BuildFailure()
        {
            var msg = new DataRowsMessage(ComparisonStyle.ByIndex, Configuration.FailureReportProfile);
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
            if (actual is IResultSetService)
                return Matches(((IResultSetService)actual).Execute());
            else if (actual is ResultSet)
            {
                actualResultSet = (ResultSet)actual;
                return Matches(actualResultSet.Rows.Count);
            }
            else if (actual is int)
                return doMatch(((int)actual));
            else
                return false;
        }

        protected virtual bool doMatch(int actual)
        {
            this.actual = actual;
            return child.Matches(actual);
        }
       
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("count of rows returned by the query is");
            child.WriteDescriptionTo(writer);
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
            child.WriteActualValueTo(writer);
        }
    }
}