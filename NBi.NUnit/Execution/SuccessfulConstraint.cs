using System;
using System.Data;
using NBi.Core;
using NBi.Core.Query;
using NBi.Extensibility;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Execution
{
    public class SuccessfulConstraint : NBiConstraint
    {
        
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IExecutionResult Result;

        public SuccessfulConstraint()
        { }

        public override bool Matches(object actual)
        {
            if (actual is IExecutable)
                return doMatch((IExecutable)actual);
            else
                return false;               
        }

        protected bool doMatch(IExecutable actual)
        {
            Result = actual.Execute();
            return Result.IsSuccess;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine();
            sb.AppendFormat("Successful execution of the etl.");
            sb.AppendLine();
            writer.WritePredicate(sb.ToString());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteActualValue(string.Format("Failure during execution of the etl: {0}", Result.Message));
        }
    }
}