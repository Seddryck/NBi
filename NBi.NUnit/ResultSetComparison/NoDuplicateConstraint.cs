using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Framework;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Analyzer;
using NBi.Framework.FailureMessage.Markdown;

namespace NBi.NUnit.Query
{
    public class NoDuplicateConstraint : NBiConstraint
    {
        protected ResultSet actualResultSet;
        private IDataRowsMessageFormatter failure;
        
        protected internal virtual DuplicatedRowsFinder Engine
        {
            get
            {
                return new DuplicatedRowsFinderByIndex();
            }
        }

        public NoDuplicateConstraint()
        { }

        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An IDbCommand or a result-set or the path to a file containing a result-set</param>
        /// <returns>true, if the result-set has unique rows</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            actualResultSet = new ResultSetBuilder().Build(actual);
            var result = Engine.Execute(actualResultSet);

            if (!result.HasNoDuplicate)
            {
                var factory = new DataRowsMessageFormatterFactory();
                var failure = factory.Instantiate(Configuration.FailureReportProfile, ComparisonStyle.ByIndex);
                failure.BuildDuplication(actualResultSet.Rows.Cast<DataRow>(), result);
            }

            return result.HasNoDuplicate;
        }

        #region "Error report"

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteLine("No duplicated row.");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteLine(failure.RenderActual());
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("Execution of the query returns duplicated rows");
            writer.WriteLine();
            writer.WriteLine();
            base.WriteMessageTo(writer);
            writer.WriteLine();
            writer.WriteLine(failure.RenderAnalysis());
        }

        #endregion

    }
}
