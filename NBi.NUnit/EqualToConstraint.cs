using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class EqualToConstraint : NUnitCtr.Constraint
    {
        /// <summary>
        /// Engine dedicated to result set comparaison
        /// </summary>
        protected IResultSetComparer _engine;

        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected Result _res;

        protected string expectedResultSetPath;
        protected IDbCommand expectedResultSetCommand;
        protected string persistenceExpectedResultSetPath;
        protected string persistenceExpectedResultSetFilename;

        public EqualToConstraint()
        {

        }

        public EqualToConstraint(IResultSetComparer resultSetComparer)
        {
            _engine = resultSetComparer;
        }

        public EqualToConstraint ExpectedResultSetPath(string value)
        {
            this.expectedResultSetPath = value;
            return this;
        }

        public EqualToConstraint ExpectedResultSetCommand(IDbCommand value)
        {
            this.expectedResultSetCommand = value;
            return this;
        }

        public EqualToConstraint PersistenceExpectedResultSetPath(string value)
        {
            this.persistenceExpectedResultSetPath = value;
            return this;
        }

        public EqualToConstraint PersistenceExpectedResultSetFilename(string value)
        {
            this.persistenceExpectedResultSetFilename = value;
            return this;
        }

        protected IResultSetComparer GetEngine(IDbCommand actual)
        {
            if (_engine == null)
                _engine = (IResultSetComparer)(QueryEngineFactory.Get(actual));
            return _engine;
        }

        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An OleDbCommand, SqlCommand or AdomdCommand</param>
        /// <returns>true, if the result of query execution is exactly identical to the content of the resultset</returns>
        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(OleDbCommand) || actual.GetType() == typeof(SqlCommand) || actual.GetType() == typeof(AdomdCommand))
                return Matches((IDbCommand)actual);
            else
                return false;

        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the engine
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public bool Matches(IDbCommand actual)
        {
            _res = GetEngine(actual).Validate(actual);
            return _res.ToBoolean();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution of the query doesn't match the expected result");
            foreach (var failure in _res.Failures)
            {
                sb.AppendLine(failure);
            }
            writer.WritePredicate(sb.ToString());
            //writer.WriteExpectedValue("");
        }
    }
}
