using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
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
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected internal IQueryParser Engine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _engine = value;
            }
        }
        
        
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected ParserResult _res;

        public SyntacticallyCorrectConstraint()
        {
        }

        protected IQueryParser GetEngine(IDbCommand actual)
        {
            if (_engine == null)
                _engine = new QueryEngineFactory().GetParser(actual);
            return _engine;
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(SqlCommand) || actual.GetType() == typeof(OleDbCommand) || actual.GetType() == typeof(AdomdCommand) )
                return doMatch((IDbCommand)actual);
            else
                return false;               
        }
        
        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        protected bool doMatch(IDbCommand actual)
        {
            _res= GetEngine(actual).Parse();
            return _res.IsSuccesful;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Query is not syntactically correct.");
            foreach (var failure in _res.Errors)
            {
                sb.AppendLine(failure);    
            }
            writer.WritePredicate(sb.ToString());
        }
    }
}