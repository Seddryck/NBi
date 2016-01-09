using System;
using NBi.Core;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.ResultSet;
using System.Data;
using NBi.Core.Calculation;

namespace NBi.NUnit.Query
{
    public class RowCountConstraint : NBiConstraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected ResultSet actualResultSet;
        protected NUnitCtr.Constraint child;
        protected IResultSetFilter filter = ResultSetFilter.None;
        protected bool isPercentage = false;

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

        public IResultSetFilter Filter
        {
            get
            {
                return filter;
            }
        }

        public RowCountConstraint IsPercentage()
        {
            this.isPercentage = true;
            return this;
        }

        public RowCountConstraint With(IResultSetFilter filter)
        {
            this.filter = filter;
            return this;
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
                var rs = Filter.Apply(actualResultSet);
                return Matches(rs.Rows.Count);
            }
            else if (actual is int)
                if (isPercentage)
                    return doMatch(((int)actual), actualResultSet.Rows.Count);
                else
                    return doMatch(((int)actual));
            else
                return false;
        }

        protected bool doMatch(int actual)
        {
            this.actual = actual;
            return child.Matches(actual);
        }

        protected bool doMatch(int actual, int original)
        {
            this.actual = actual/original;
            return child.Matches(actual);
        }
       
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("execution of the query ");
            if (filter != ResultSetFilter.None)
                sb.Append("and application of the filter ");
            sb.Append("returns a row-count");
            if (child is NUnitCtr.EqualConstraint)
                sb.Append(" equal to");
            writer.WritePredicate(sb.ToString());
            child.WriteDescriptionTo(writer);
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