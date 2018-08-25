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
    public class LookupAnalyzer
    {
        private readonly ColumnMappingCollection settings;

        public LookupAnalyzer(ColumnMappingCollection settings)
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
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, string.Format("Building collection of keys from parent: {0} [{1}]", references.Count, stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            stopWatch.Reset();
            var candidateKeyBuilder = BuildKeysRetriever(settings, x => x.CandidateColumn);
            var violations = ExtractReferenceViolation(child, candidateKeyBuilder, references);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, string.Format("Analyzing potential reference violation for {0} rows [{1}]", child.Rows.Count, stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

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

            if (settings.Any(x => target(x) is ColumnPositionIdentifier))
                return new KeysRetrieverByIndex(defColumns);
            else
                return new KeysRetrieverByName(defColumns);
        }

        private IList<KeyCollection> BuildReferences(DataTable table, KeysRetriever keyRetriever)
        {

            var references = new List<KeyCollection>();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetKeys(row);
                if (!references.Contains(keys))
                    references.Add(keys);
            }

            return references;
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
