using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework;
using NUnit.Framework;

namespace NBi.NUnit.Query
{
    public class UniqueRowsConstraint : NBiConstraint
    {
        protected ResultSet actualResultSet;
        private IDataRowsMessageFormatter failure;

        protected Evaluator Engine { get; set; }

        public UniqueRowsConstraint()
        {
            Engine = new IndexEvaluator();
        }

        public UniqueRowsConstraint Using(Evaluator evaluator)
        {
            this.Engine = evaluator;
            return this;
        }

        /// <summary>
        /// Handle a IResultSetService execute it and check if the result contains unique rows or not
        /// </summary>
        /// <param name="actual">An IResultSetService or a result-set</param>
        /// <returns>true, if the result-set has unique rows</returns>
        public override bool Matches(object actual)
        {
            if (actual is IResultSetService)
            {
                return Matches((actual as IResultSetService).Execute());
            }
            else if (actual is ResultSet)
            {
                actualResultSet = (ResultSet)actual;
                var result = Engine.Execute(actualResultSet);

                if (!result.AreUnique || Configuration.FailureReportProfile.Mode == FailureReportMode.Always)
                {
                    var factory = new DataRowsMessageFormatterFactory();
                    failure = factory.Instantiate(Configuration.FailureReportProfile, Engine is IndexEvaluator ? EngineStyle.ByIndex : EngineStyle.ByName);
                    failure.BuildDuplication(actualResultSet.Rows.Cast<DataRow>(), result);
                }

                if (result.AreUnique && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
                    Assert.Pass(failure.RenderMessage());

                return result.AreUnique;
            }
            else
                throw new ArgumentException();

        }

        #region "Error report"

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration?.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine("No duplicated row.");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration?.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine(failure.RenderActual());
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration?.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(failure.RenderMessage());
            else
            {
                writer.WritePredicate("Execution of the query returns duplicated rows");
                writer.WriteLine();
                writer.WriteLine();
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine(failure.RenderAnalysis());
            }
        }

        #endregion

    }
}
