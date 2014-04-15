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
        protected IQueryParser engine;
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected internal IQueryParser Engine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                engine = value;
            }
        }
        
        
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected ParserResult parserResult;

        public SyntacticallyCorrectConstraint()
        {
        }

        protected IQueryParser GetEngine(IDbCommand actual)
        {
            if (engine == null)
                engine = new QueryEngineFactory().GetParser(actual);
            return engine;
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        public override bool Matches(object actual)
        {
            if (actual is IDbCommand)
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
            parserResult= GetEngine(actual).Parse();
            return parserResult.IsSuccesful;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Query is not syntactically correct.");
            foreach (var failure in parserResult.Errors)
            {
                sb.AppendLine(failure);    
            }
            writer.WritePredicate(sb.ToString());
        }
    }
}