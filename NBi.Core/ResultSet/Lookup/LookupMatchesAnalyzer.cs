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
    public class LookupMatchesAnalyzer : LookupExistsAnalyzer
    {
        protected ColumnMappingCollection Values { get; private set; }

        public LookupMatchesAnalyzer(ColumnMappingCollection keys, ColumnMappingCollection values)
            : base(keys)
        {
            Values = values;
        }

        protected override LookupViolations Execute(DataTable candidate, DataTable reference)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var referenceKeyRetriever = BuildColumnsRetriever(Keys, x => x.ReferenceColumn);
            var referenceValueRetriever = BuildColumnsRetriever(Values, x => x.ReferenceColumn);
            var references = BuildReferenceIndex(reference, referenceKeyRetriever, referenceValueRetriever);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Building the index for keys from reference table (including value columns) containing {references.Count} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            stopWatch.Restart();
            var candidateKeyBuilder = BuildColumnsRetriever(Keys, x => x.CandidateColumn);
            var candidateValueRetriever = BuildColumnsRetriever(Values, x => x.CandidateColumn);
            var violations = ExtractLookupViolation(candidate, candidateKeyBuilder, candidateValueRetriever, references);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Analyzing potential lookup violations based on keys and values for {candidate.Rows.Count} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            return violations;
        }

        protected IDictionary<KeyCollection, ICollection<KeyCollection>> BuildReferenceIndex(DataTable table, CellsRetriever keyRetriever, CellsRetriever valuesRetriever)
        {
            var references = new Dictionary<KeyCollection, ICollection<KeyCollection>>();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetColumns(row);
                var values = valuesRetriever.GetColumns(row);
                if (!references.ContainsKey(keys))
                    references.Add(keys, new HashSet<KeyCollection>() { values });
                else
                    references[keys].Add(values);
            }

            return references;
        }

        private LookupViolations ExtractLookupViolation(DataTable table, CellsRetriever keyRetriever, CellsRetriever valueRetriever, IDictionary<KeyCollection, ICollection<KeyCollection>> references)
        {
            var violations = new LookupViolations();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetColumns(row);
                if (!references.ContainsKey(keys))
                {
                    if (violations.ContainsKey(keys))
                        violations[keys].Add(row);
                    else
                        violations.Add(keys, new Collection<DataRow>() { row });
                }
                else
                {
                    var values = valueRetriever.GetColumns(row);
                    if (!references[keys].Contains(values))
                        violations.Add(keys, new Collection<DataRow>() { row });
                }
            }
            return violations;
        }
    }
}
