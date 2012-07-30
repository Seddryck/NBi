using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class EqualToConstraint : NUnitCtr.Constraint
    {
        protected Object expect;
        protected string persistenceExpectedResultSetFullPath;
        protected string persistenceActualResultSetFullPath;
     
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected IResultSetComparer _engine;
        protected internal IResultSetComparer Engine
        {
            get
            {
                if(_engine==null)
                    _engine = new DataRowBasedResultSetComparer();
                return _engine;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException();
                _engine = value;
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
                if(_resultSetBuilder==null)
                    _resultSetBuilder = new ResultSetBuilder();
                return _resultSetBuilder;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException();
                _resultSetBuilder = value;
            }
        }
        
        public EqualToConstraint (string value)
        {
            this.expect = value;
        }

        public EqualToConstraint (ResultSet value)
        {
            this.expect = value;
        }

        public EqualToConstraint(IEnumerable<IRow> value)
        {
            this.expect = value;
        }

        public EqualToConstraint (IDbCommand value)
        {
            this.expect = value;
        }

        public EqualToConstraint Using(ResultSetComparaisonSettings settings)
        {
            this.Engine.Settings = settings;
            return this;
        }

        public EqualToConstraint PersistExpectation(string path, string filename)
        {
            this.persistenceExpectedResultSetFullPath = Path.Combine(path, filename);
            return this;
        }


        public EqualToConstraint PersistActual(string path, string filename)
        {
            this.persistenceActualResultSetFullPath = Path.Combine(path, filename);
            return this;
        }

        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An OleDbCommand, SqlCommand or AdomdCommand</param>
        /// <returns>true, if the result of query execution is exactly identical to the content of the resultset</returns>
        public override bool Matches(object actual)
        {
            if (actual is IDbCommand)
                return Process((IDbCommand)actual);
            else if (actual is ResultSet)
                return doMatch((ResultSet)actual);
            else
                return false;

        }

        protected bool doMatch(ResultSet actual)
        {
            var rsExpect = GetResultSet(expect);
            return Engine.Compare(actual, rsExpect).Difference == ResultSetDifferenceType.None;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution of the query doesn't match the expected result");
            writer.WritePredicate(sb.ToString());
        }
    }
}
