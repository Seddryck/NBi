using NBi.Core.Scalar.Casting;
using NBi.Core.ResultSet.Lookup.Violation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Lookup;

public class LookupExistsAnalyzer : ILookupAnalyzer
{
    protected ColumnMappingCollection Keys { get; private set; }

    public LookupExistsAnalyzer(ColumnMappingCollection keys)
    {
        Keys = keys;
    }

    public virtual LookupViolationCollection Execute(object candidate, object reference)
    {
        if (candidate is DataTable candidateTable && reference is DataTable referenceTable)
            return Execute(candidateTable, referenceTable);

        if (candidate is IResultSet candidateRs && reference is IResultSet referenceRs)
            return Execute(candidateRs, referenceRs);

        throw new ArgumentException();
    }

    protected virtual LookupViolationCollection Execute(IResultSet candidate, IResultSet reference)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var referenceKeyRetriever = BuildColumnsRetriever(Keys, x => x.ReferenceColumn);
        var references = BuildReferenceIndex(reference, referenceKeyRetriever);
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Building the index for keys from reference table containing {references.Count()} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

        stopWatch.Restart();
        var candidateKeyBuilder = BuildColumnsRetriever(Keys, x => x.CandidateColumn);
        var violations = ExtractLookupViolation(candidate, candidateKeyBuilder, references);
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Analyzing potential lookup violations (based on keys) for the {candidate.RowCount} rows from candidate table [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

        return violations;
    }

    protected CellRetriever BuildColumnsRetriever(ColumnMappingCollection columns, Func<ColumnMapping, IColumnIdentifier> target)
    {
        var defColumns = new Collection<IColumnDefinition>();
        foreach (var column in columns)
        {
            var defColumn = column.ToColumnDefinition(() => target(column));
            defColumns.Add(defColumn);
        }

        if (columns.Any(x => target(x) is ColumnOrdinalIdentifier))
            return new CellRetrieverByOrdinal(defColumns);
        else
            return new CellRetrieverByName(defColumns);
    }

    protected IEnumerable<KeyCollection> BuildReferenceIndex(IResultSet table, CellRetriever keyRetriever)
    {
        var references = new HashSet<KeyCollection>();

        foreach (var row in table.Rows)
        {
            var keys = keyRetriever.GetColumns(row);
            if (!references.Contains(keys))
                references.Add(keys);
        }

        return references;
    }

    protected virtual LookupViolationCollection ExtractLookupViolation(IResultSet table, CellRetriever keyRetriever, IEnumerable<KeyCollection> references)
    {
        var violations = new LookupExistsViolationCollection(Keys);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var i = 0;

        foreach (var row in table.Rows)
        {
            i++;
            
            var keys = keyRetriever.GetColumns(row);
            if (!references.Contains(keys))
                violations.Register(keys, row);
            if (i % 1000 == 0)
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Searching for {i} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");
        }
        return violations;
    }
}
