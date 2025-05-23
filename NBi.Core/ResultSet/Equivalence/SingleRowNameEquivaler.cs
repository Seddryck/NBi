﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet.Analyzer;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Equivalence;

internal class SingleRowNameEquivaler : NameEquivaler
{
    public SingleRowNameEquivaler(SettingsSingleRowNameResultSet settings)
        : base(AnalyzersFactory.EqualTo(), settings)
    {}

    public override ResultResultSet Compare(IResultSet x, IResultSet y)
    {
        if (x.RowCount > 1)
            throw new ArgumentException($"The query in the assertion returns {x.RowCount} rows. It was expected to return zero or one row.");

        if (y.RowCount > 1)
            throw new ArgumentException($"The query in the system-under-test returns {y.RowCount} rows. It was expected to return zero or one row.");

        if (x.RowCount == 1 && y.RowCount == 1)
            PreliminaryChecks(x, y);

        return Compare(x.RowCount == 1 ? x[0] : null, y.RowCount == 1 ? y[0] : null);
    }

    protected ResultResultSet Compare(IResultRow? x, IResultRow? y)
    {
        var chrono = DateTime.Now;

        var missingRows = new List<IResultRow>();
        var unexpectedRows = new List<IResultRow>();

        if (x == null && y != null)
            unexpectedRows.Add(y);

        if (x != null && y == null)
            missingRows.Add(x);
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

        var nonMatchingValueRows = new List<IResultRow>();
        if (missingRows.Count == 0 && unexpectedRows.Count == 0)
        {
            chrono = DateTime.Now;
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length and format of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            // If all of the columns make up the key, then we already know which rows match and which don't.
            //  So there is no need to continue testing
            chrono = DateTime.Now;
            var nonMatchingValueRow = CompareRows(x!, y!);
            if (nonMatchingValueRow!=null)
                nonMatchingValueRows.Add(nonMatchingValueRow);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key but without matching value: {0} [{1}]", nonMatchingValueRows.Count, DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
        }

        return ResultResultSet.Build(
            missingRows,
            unexpectedRows,
            [],
            [],
            nonMatchingValueRows
            );
    }
}
