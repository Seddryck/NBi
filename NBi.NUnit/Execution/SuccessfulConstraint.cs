using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NBi.Core.Query;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Execution
{
    public class SuccessfulConstraint : NUnitCtr.Constraint
    {
        
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IExecutionResult Result;

        public SuccessfulConstraint()
        {
        }


        public override bool Matches(object actual)
        {
            if (actual is IExecution)
                return doMatch((IExecution)actual);
            else
                return false;               
        }

        protected bool doMatch(IExecution actual)
        {
            Result = actual.Run();
            return Result.IsSuccess;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution wasn't successful.");
            sb.AppendLine(Result.Message);
            writer.WritePredicate(sb.ToString());
        }
    }
}