using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Core.Scalar.Casting;
using NBi.Core.Scalar.Comparer;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup;

public class LookupMatchesAnalyzer : LookupExistsAnalyzer
{
    protected ColumnMappingCollection Values { get; private set; }
    protected IDictionary<IColumnIdentifier, Tolerance> Tolerances { get; private set; }

    public LookupMatchesAnalyzer(ColumnMappingCollection keys, ColumnMappingCollection values)
        : this(keys, values, new Dictionary<IColumnIdentifier, Tolerance>()) { }

    public LookupMatchesAnalyzer(ColumnMappingCollection keys, ColumnMappingCollection values, IDictionary<IColumnIdentifier, Tolerance> tolerances)
        : base(keys)
    {
        Values = values;
        Tolerances = tolerances ?? new Dictionary<IColumnIdentifier, Tolerance>();
    }

    protected override LookupViolationCollection Execute(IResultSet candidate, IResultSet reference)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var referenceKeyRetriever = BuildColumnsRetriever(Keys, x => x.ReferenceColumn);
        var referenceValueRetriever = BuildColumnsRetriever(Values, x => x.ReferenceColumn);
        var references = BuildReferenceIndex(reference, referenceKeyRetriever, referenceValueRetriever);
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Building the index (including value columns) for keys from the reference table containing {references.Count} rows [{stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]");

        stopWatch.Restart();
        var candidateKeyBuilder = BuildColumnsRetriever(Keys, x => x.CandidateColumn);
        var candidateValueRetriever = BuildColumnsRetriever(Values, x => x.CandidateColumn);
        var violations = ExtractLookupViolation(candidate, candidateKeyBuilder, candidateValueRetriever, references, Tolerances);
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Analyzing potential lookup violations (based on keys and values) for the {candidate.RowCount} rows from candidate table [{stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]");

        return violations;
    }

    protected IDictionary<KeyCollection, ICollection<KeyCollection>> BuildReferenceIndex(IResultSet table, CellRetriever keyRetriever, CellRetriever valuesRetriever)
    {
        var references = new Dictionary<KeyCollection, ICollection<KeyCollection>>();

        foreach (var row in table.Rows)
        {
            var keys = keyRetriever.GetColumns(row);
            var values = valuesRetriever.GetColumns(row);
            if (!references.TryGetValue(keys, out var value))
                references.Add(keys, [values]);
            else
                value.Add(values);
        }

        return references;
    }

    private LookupViolationCollection ExtractLookupViolation(IResultSet table, CellRetriever keyRetriever, CellRetriever valueRetriever, IDictionary<KeyCollection, ICollection<KeyCollection>> references, IDictionary<IColumnIdentifier, Tolerance> tolerances)
    {
        var violations = new LookupMatchesViolationCollection(Keys, Values);

        foreach (var row in table.Rows)
        {
            var keys = keyRetriever.GetColumns(row);
            if (!references.TryGetValue(keys, out var value))
                violations.Register(keys, row);
            else
            {
                var setResults = new List<Dictionary<IResultColumn, ComparerResult>>();
                foreach (var valueFields in value)
                {
                    var rowResults = new Dictionary<IResultColumn, ComparerResult>();
                    var tuples = valueFields.Members.Zip(Values,
                        (x, c) => new
                        {
                            ReferenceValue = x,
                            CandidateValue = row[c.CandidateColumn],
                            c.Type,
                            Column = table.GetColumn(c.CandidateColumn),
                            Tolerance = tolerances.TryGetValue(c.CandidateColumn, out var value) ? value : null
                        });

                    foreach (var tuple in tuples)
                    {
                        var cellComparer = new CellComparer();
                        var cellResult = cellComparer.Compare(tuple.ReferenceValue, tuple.CandidateValue ?? throw new NullReferenceException(), tuple.Type, tuple.Tolerance, null);
                        rowResults.Add(tuple.Column ?? throw new NullReferenceException(), cellResult);
                    }
                    setResults.Add(rowResults);

                    if (rowResults.Values.All(x => x.AreEqual))
                        break;
                }

                if (!setResults.Any(x => x.All(y => y.Value.AreEqual)))
                {
                    var composite = new LookupMatchesViolationComposite(row, []);
                    foreach (var rowResults in setResults)
                    {
                        var cases = new LookupMatchesViolationRecord();
                        foreach (var cellResult in rowResults)
                        {
                            var data = new LookupMatchesViolationData(cellResult.Value.AreEqual, cellResult.Value.Message);
                            cases.Add(cellResult.Key, data);
                        }
                        composite.Records.Add(cases);
                    }
                    violations.Register(keys, composite);
                }
            }
        }
        return violations;
    }
}
