using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Uniqueness;

namespace NBi.NUnit.Query
{
    public class UniqueRowsConstraint : NBiConstraint
    {
        protected ResultSet actualResultSet;
        private DataRowsMessage failure;
        
        protected internal virtual UniqueRowsFinder Engine
        {
            get
            {
                return new UniqueRowsFinderByIndex();
            }
        }

        public UniqueRowsConstraint()
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

            if (!result.AreUnique)
            {
                failure = new DataRowsMessage(ComparisonStyle.ByIndex, Configuration.FailureReportProfile);
                failure.BuildDuplication(actualResultSet.Rows.Cast<DataRow>(), result);
            }

            return result.AreUnique;
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
            writer.WriteLine(failure.RenderDuplicated());
        }

        #endregion

    }
}
