using System;
using System.Data;
using NBi.Core.Query;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Query
{
    public class FasterThanConstraint : NUnitCtr.Constraint
    {
        protected IQueryPerformance engine;
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected internal IQueryPerformance Engine
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
        protected PerformanceResult performanceResult;

        protected int maxTimeMilliSeconds;
        protected bool cleanCache;

        public FasterThanConstraint()
        {

        }

        public FasterThanConstraint MaxTimeMilliSeconds(int value)
        {
            this.maxTimeMilliSeconds = value;
            return this;
        }

        public FasterThanConstraint CleanCache()
        {
            this.cleanCache = true;
            return this;
        }

        protected IQueryPerformance GetEngine(IDbCommand actual)
        {
            if (engine == null)
                engine = new QueryEngineFactory().GetPerformance(actual);
            return engine;
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
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
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public bool doMatch(IDbCommand actual)
        {
            var engine = GetEngine(actual);
            if (cleanCache)
                engine.CleanCache();
            performanceResult = engine.CheckPerformance();
            return performanceResult.TimeElapsed.TotalMilliseconds < maxTimeMilliSeconds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution of the query is slower than expected");
            sb.AppendFormat("Maximum expected was {0}ms", maxTimeMilliSeconds);
            writer.WritePredicate(sb.ToString());           
        }

        public override void  WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteActualValue(string.Format("{0}ms", performanceResult.TimeElapsed.TotalMilliseconds));
        }
    }
}