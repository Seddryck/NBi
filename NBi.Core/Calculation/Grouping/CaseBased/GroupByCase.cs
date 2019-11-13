using NBi.Core.Calculation.Predication;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Grouping.CaseBased
{
    class GroupByCase
    {
        protected IEnumerable<IPredication> Cases { get; }

        public GroupByCase(IEnumerable<IPredication> cases)
            => Cases = cases;

        public IDictionary<object, DataTable> Execute(ResultSet.ResultSet resultSet, Context context)
        {
            var stopWatch = new Stopwatch();
            var dico = new Dictionary<object, DataTable>();
            stopWatch.Start();

            foreach (DataRow row in resultSet.Rows)
            {
                context.Switch(row);
                var predication = Cases.FirstOrDefault(p => p.Execute(context));

                if (!dico.ContainsKey(predication))
                    dico.Add(predication, row.Table.Clone());
                dico[predication].ImportRow(row);
            }

            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Building rows' groups by cases: {dico.Count} [{stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");
            return dico;

        }
    }
}
