using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NBi.Core.Database;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class SyntacticallyCorrectConstraint : NUnitCtr.Constraint
    {
        /// <summary>
        /// Engine dedicated to query parsing
        /// </summary>
        protected IQueryParser _engine;
        
        
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected Result _res;

        /// <summary>
        /// .ctor, define the default engine used by this constraint
        /// </summary>
        /// <param name="connectionString">Connection string used to connect to the server where the constraint will be tested</param>
        public SyntacticallyCorrectConstraint()
        {
            _engine = new QueryParser();
        }

        /// <summary>
        /// .ctor mainly used for mocking
        /// </summary>
        /// <param name="engine">The engine to use</param>
        protected internal SyntacticallyCorrectConstraint(IQueryParser engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(SqlCommand) || actual.GetType() == typeof(OleDbCommand) || actual.GetType() == typeof(AdomdCommand) )
                return Matches((IDbCommand)actual);
            else
                return false;               
        }
        
        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        public bool Matches(IDbCommand actual)
        {
            _res= _engine.Validate(actual);
            return _res.ToBoolean();
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Parsing of the query failed");
            foreach (var failure in _res.Failures)
            {
                sb.AppendLine(failure);    
            }
            writer.WritePredicate(sb.ToString());
            //writer.WriteExpectedValue("");
        }
    }
}