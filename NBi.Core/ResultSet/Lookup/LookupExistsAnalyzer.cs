using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup
{
    public class LookupExistsAnalyzer : ILookupAnalyzer
    {
        protected ColumnMappingCollection Keys { get; private set; }

        public LookupExistsAnalyzer(ColumnMappingCollection keys)
        {
            Keys = keys;
        }

        public virtual LookupViolations Execute(object candidate, object reference)
        {
            if (candidate is DataTable && reference is DataTable)
                return Execute((DataTable)candidate, (DataTable)reference);

            if (candidate is ResultSet && reference is ResultSet)
                return Execute(((ResultSet)candidate).Table, ((ResultSet)reference).Table);

            throw new ArgumentException();
        }

        protected virtual LookupViolations Execute(DataTable candidate, DataTable reference)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var referenceKeyRetriever = BuildColumnsRetriever(Keys, x => x.ReferenceColumn);
            var references = BuildReferenceIndex(reference, referenceKeyRetriever);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Building the index for keys from reference table containing {references.Count()} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            stopWatch.Restart();
            var candidateKeyBuilder = BuildColumnsRetriever(Keys, x => x.CandidateColumn);
            var violations = ExtractLookupViolation(candidate, candidateKeyBuilder, references);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Analyzing potential lookup violations (based on keys) for the {candidate.Rows.Count} rows from candidate table [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            return violations;
        }

        protected CellsRetriever BuildColumnsRetriever(ColumnMappingCollection columns, Func<ColumnMapping, IColumnIdentifier> target)
        {
            var defColumns = new Collection<IColumnDefinition>();
            foreach (var column in columns)
            {
                var defColumn = column.ToColumnDefinition(() => target(column));
                defColumns.Add(defColumn);
            }

            if (columns.Any(x => target(x) is ColumnOrdinalIdentifier))
                return new CellsRetrieverByOrdinal(defColumns);
            else
                return new CellsRetrieverByName(defColumns);
        }

        protected IEnumerable<KeyCollection> BuildReferenceIndex(DataTable table, CellsRetriever keyRetriever)
        {
            var references = new HashSet<KeyCollection>();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetColumns(row);
                if (!references.Contains(keys))
                    references.Add(keys);
            }

            return references.ToList();
        }

        protected virtual LookupViolations ExtractLookupViolation(DataTable table, CellsRetriever keyRetriever, IEnumerable<KeyCollection> references)
        {
            var violations = new LookupViolations();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetColumns(row);
                if (!references.Contains(keys))
                {
                    if (violations.ContainsKey(keys))
                        violations[keys].Add(row);
                    else
                        violations.Add(keys, new Collection<DataRow>() { row });
                }
            }
            return violations;
        }
    }
}
