using System;
using NBi.Core;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Execution
{
    public class FasterThanConstraint : NUnitCtr.Constraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IExecutionResult Result;

        protected int maxTimeMilliSeconds;
        protected int timeOutMilliSeconds;
        protected bool cleanCache;

        public FasterThanConstraint()
        {

        }

        public FasterThanConstraint MaxTimeMilliSeconds(int value)
        {
            this.maxTimeMilliSeconds = value;
            return this;
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public override bool Matches(object actual)
        {
            if (actual is IExecution)
                return doMatch((IExecution)actual);
            else
                return false;
        }

        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public bool doMatch(IExecution actual)
        {
            Result = actual.Run();
            return 
                (
                    Result.TimeElapsed.TotalMilliseconds < maxTimeMilliSeconds
                );
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
            writer.WriteActualValue(string.Format("{0}ms", Result.TimeElapsed.TotalMilliseconds));
        }
    }
}