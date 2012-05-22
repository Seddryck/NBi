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
        /// <summary>
        /// Engine dedicated to query parsing
        /// </summary>
        protected IQueryPerformance _engine;

        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected PerformanceResult _res;

        protected int _maxTimeMilliSeconds;
        protected bool _cleanCache;

        /// .ctor, define the default engine used by this constraint
        /// </summary>
        /// <param name="maxTimeMilliSeconds">The query should run faster than the maximum time specified here</param>
        /// <param name="cleanCache">Specify if the cache needs to be cleant or not</param>
        public FasterThanConstraint(int maxTimeMilliSeconds, bool cleanCache)
        {
            _maxTimeMilliSeconds= maxTimeMilliSeconds;
            _cleanCache = cleanCache;
        }

        /// <summary>
        /// .ctor mainly used for mocking
        /// </summary>
        /// <param name="engine">The engine to use</param>
        protected internal FasterThanConstraint(IQueryPerformance engine)
        {
            _engine = engine;
        }

        public FasterThanConstraint MaxTimeMilliSeconds(int value)
        {
            this._maxTimeMilliSeconds = value;
            return this;
        }

        protected IQueryPerformance GetEngine(IDbCommand actual)
        {
            if (_engine == null)
                _engine = (IQueryPerformance)(QueryEngineFactory.Get(actual));
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
            _res = GetEngine(actual).CheckPerformance(actual, _cleanCache);
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