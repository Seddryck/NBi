using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet.Analyzer;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Equivalence
{
    internal class SingleRowNameEquivaler : NameEquivaler
    {
        private new SettingsSingleRowNameResultSet  Settings
        {
            get { return base.Settings as SettingsSingleRowNameResultSet; }
        }
        
        public SingleRowNameEquivaler(SettingsSingleRowNameResultSet settings)
            : base(AnalyzersFactory.EqualTo(), settings)
        {}

        protected override ResultResultSet doCompare(IResultSet x, IResultSet y)
        {
            if (x.Rows.Count > 1)
                throw new ArgumentException($"The query in the assertion returns {x.Rows.Count} rows. It was expected to return zero or one row.");

            if (y.Rows.Count > 1)
                throw new ArgumentException($"The query in the system-under-test returns {y.Rows.Count} rows. It was expected to return zero or one row.");

            if (x.Rows.Count == 1 && y.Rows.Count == 1)
                PreliminaryChecks(x, y);

            return doCompare(x.Rows.Count == 1 ? x.Rows[0] : null, y.Rows.Count == 1 ? y.Rows[0] : null);
        }

        protected ResultResultSet doCompare(DataRow x, DataRow y)
        {
            var chrono = DateTime.Now;

            var missingRows = new List<DataRow>();
            var unexpectedRows = new List<DataRow>();

            if (x == null && y != null)
                unexpectedRows.Add(y);

            if (x != null && y == null)
                missingRows.Add(x);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            IList<DataRow> nonMatchingValueRows = new List<DataRow>();
            if (missingRows.Count == 0 && unexpectedRows.Count == 0)
            {
                chrono = DateTime.Now;
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length and format of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

                // If all of the columns make up the key, then we already know which rows match and which don't.
                //  So there is no need to continue testing
                chrono = DateTime.Now;
                var nonMatchingValueRow = CompareRows(x, y);
                if (nonMatchingValueRow!=null)
                    nonMatchingValueRows.Add(nonMatchingValueRow);
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key but without matching value: {0} [{1}]", nonMatchingValueRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            }

            return ResultResultSet.Build(
                missingRows,
                unexpectedRows,
                new List<DataRow>(),
                new List<DataRow>(),
                nonMatchingValueRows
                );
        }
    }
}
