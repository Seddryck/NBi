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

        /// <summary>
        /// Engine dedicated to ResultSet acquisition
        /// </summary>
        protected IResultSetBuilder _resultSetBuilder;
        protected internal IResultSetBuilder ResultSetBuilder
        {
            get
            {
                if (_resultSetBuilder == null)
                    _resultSetBuilder = new ResultSetBuilder();

                return _resultSetBuilder;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _resultSetBuilder = value;
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
        /// <param name="actual">An OleDbCommand, SqlCommand or AdomdCommand</param>
        /// <returns>true, if the row-count of query execution validates the child constraint</returns>
        public override bool Matches(object actual)
        {
            if (actual is IDbCommand)
                return Process((IDbCommand)actual);
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

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IDbCommand actual)
        {
            var rsActual = GetResultSet(actual);
            return this.Matches(rsActual);
        }

        protected ResultSet GetResultSet(Object obj)
        {
            return ResultSetBuilder.Build(obj);
        }

    }
}