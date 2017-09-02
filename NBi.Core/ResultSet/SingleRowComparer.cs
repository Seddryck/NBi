using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using System.Text;
using NBi.Core.ResultSet.Converter;

namespace NBi.Core.ResultSet
{
    public class SingleRowComparer : ResultSetComparerByIndex
    {
        private SettingsSingleRowComparison settings
        {
            get { return Settings as SettingsSingleRowComparison; }
        }
        
        public SingleRowComparer(SettingsSingleRowComparison settings)
            : base(settings)
        {}

        
        protected override ResultSetCompareResult doCompare(DataTable x, DataTable y)
        {
            if (x.Rows.Count > 1)
                throw new ArgumentException(string.Format("The query in the assertion returns {0} rows. It was expected to return zero or one row.", x.Rows.Count));

            if (y.Rows.Count > 1)
                throw new ArgumentException(string.Format("The query in the system-under-test returns {0} rows. It was expected to return zero or one row.", y.Rows.Count));

            return doCompare(x.Rows.Count == 1 ? x.Rows[0] : null, y.Rows.Count == 1 ? y.Rows[0] : null);
        }

        protected ResultSetCompareResult doCompare(DataRow x, DataRow y)
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
                var columnsCount = Math.Max(y.Table.Columns.Count, x.Table.Columns.Count);
                if (settings == null)
                    BuildDefaultSettings(columnsCount);
                else
                    settings.ApplyTo(columnsCount);

                PreliminaryChecks(x.Table, y.Table);
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length and format of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

                // If all of the columns make up the key, then we already know which rows match and which don't.
                //  So there is no need to continue testing
                chrono = DateTime.Now;
                var nonMatchingValueRow = CompareRows(x, y);
                if (nonMatchingValueRow!=null)
                    nonMatchingValueRows.Add(nonMatchingValueRow);
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key but without matching value: {0} [{1}]", nonMatchingValueRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            }

            return ResultSetCompareResult.Build(
                missingRows,
                unexpectedRows,
                new List<DataRow>(),
                new List<DataRow>(),
                nonMatchingValueRows
                );
        }
        
    }
}
