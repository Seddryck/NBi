using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class FasterThanConstraint : NUnitCtr.Constraint
    {
        protected IQueryPerformance _engine;
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected internal IQueryPerformance Engine
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
        protected PerformanceResult _res;

        protected int _maxTimeMilliSeconds;
        protected bool _cleanCache;

        public FasterThanConstraint()
        {

        }

        public FasterThanConstraint MaxTimeMilliSeconds(int value)
        {
            this._maxTimeMilliSeconds = value;
            return this;
        }

        public FasterThanConstraint CleanCache()
        {
            this._cleanCache = true;
            return this;
        }

        protected IQueryPerformance GetEngine(IDbCommand actual)
        {
            if (_engine == null)
                _engine = new QueryEngineFactory().GetPerformance(actual);
            return _engine;
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(OleDbCommand) || actual.GetType() == typeof(SqlCommand) || actual.GetType() == typeof(AdomdCommand))
                return Matches((IDbCommand)actual);
            else
                return false;
        }

        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public bool Matches(IDbCommand actual)
        {
            var engine = GetEngine(actual);
            if (_cleanCache)
                engine.CleanCache();
            _res = engine.CheckPerformance();
            return _res.TimeElapsed.TotalMilliseconds < _maxTimeMilliSeconds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution of the query is slower than expected");
            sb.AppendFormat("Maximum expected was {0}ms and query has been exectued in {1}ms\r\n", _maxTimeMilliSeconds, _res.TimeElapsed.TotalMilliseconds);
            writer.WritePredicate(sb.ToString());
        }
    }
}