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

                if (!result.AreUnique)
                {
                    failure = new DataRowsMessage(ComparisonStyle.ByIndex, Configuration.FailureReportProfile);
                    failure.BuildDuplication(actualResultSet.Rows.Cast<DataRow>(), result);
                }

                return result.AreUnique;
            }
            else
                throw new ArgumentException();
            
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
