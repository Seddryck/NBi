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
    public class LookupExistsAnalyzer
    {
        private readonly ColumnMappingCollection settings;

        public LookupExistsAnalyzer(ColumnMappingCollection settings)
        {
            this.settings = settings;
        }

        public virtual LookupViolations Execute(object child, object parent)
        {
            if (child is DataTable && parent is DataTable)
                return Execute((DataTable)child, (DataTable)parent);

            if (child is ResultSet && parent is ResultSet)
                return Execute(((ResultSet)child).Table, ((ResultSet)parent).Table);

            throw new ArgumentException();
        }

        protected LookupViolations Execute(DataTable child, DataTable parent)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var referenceKeyRetriever = BuildKeysRetriever(settings, x => x.ReferenceColumn);
            var references = BuildReferences(parent, referenceKeyRetriever);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Building collection of keys from parent: {references.Count} [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            stopWatch.Reset();
            var candidateKeyBuilder = BuildKeysRetriever(settings, x => x.CandidateColumn);
            var violations = ExtractReferenceViolation(child, candidateKeyBuilder, references);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Analyzing potential reference violation for {child.Rows.Count} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            return violations;
        }

        private KeysRetriever BuildKeysRetriever(ColumnMappingCollection settings, Func<ColumnMapping, IColumnIdentifier> target)
        {
            var defColumns = new Collection<IColumnDefinition>();
            foreach (var setting in settings)
            {
                var defColumn = setting.ToColumnDefinition(() => target(setting));
                defColumns.Add(defColumn);
            }

            if (settings.Any(x => target(x) is ColumnOrdinalIdentifier))
                return new KeysRetrieverByOrdinal(defColumns);
            else
                return new KeysRetrieverByName(defColumns);
        }

        private IList<KeyCollection> BuildReferences(DataTable table, KeysRetriever keyRetriever)
        {
            var references = new HashSet<KeyCollection>();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetKeys(row);
                if (!references.Contains(keys))
                    references.Add(keys);
            }

            return references.ToList();
        }

        private LookupViolations ExtractReferenceViolation(DataTable table, KeysRetriever keyRetriever, IEnumerable<KeyCollection> references)
        {
            var violations = new LookupViolations();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetKeys(row);
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
