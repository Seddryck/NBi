using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication
{
    class DuplicateEngine : IDuplicationEngine
    {
        protected ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }
        protected IPredication Predication { get; }
        protected IScalarResolver<int> Times { get; }
        protected IList<OutputArgs> Outputs { get; }

        public DuplicateEngine(ServiceLocator serviceLocator, Context context, IPredication predication, IScalarResolver<int> times, IList<OutputArgs> outputs)
            => (ServiceLocator, Context, Predication, Times, Outputs) = (serviceLocator, context, predication, times, outputs);

        public IResultSet Execute(IResultSet rs)
        {
            var newTable = rs.Table.Copy();
            newTable.Clear();

            foreach (var output in Outputs)
            {
                if (newTable.Columns.Contains(output.Name))
                    throw new ArgumentException($"Can't add a new column named '{output.Name}' because this column already exists in the orginal result-set.");

                newTable.Columns.Add(new DataColumn(output.Name, typeof(object)));
            }

            foreach (DataRow row in rs.Rows)
            {
                Context.Switch(row);
                var isDuplicated = Predication.Execute(Context);
                var times = Times.Execute();

                newTable.ImportRow(row);
                foreach (var output in Outputs)
                    newTable.Rows[newTable.Rows.Count - 1][output.Name] = output.Value == OutputValue.Total 
                        ? times * Convert.ToInt32(isDuplicated) + 1
                        : 0;

                if (isDuplicated)
                {
                    for (int i = 0; i < times; i++)
                    {
                        newTable.ImportRow(row);
                        foreach (var output in Outputs)
                            newTable.Rows[newTable.Rows.Count - 1][output.Name] = output.Value == OutputValue.Total ? times + 1 : i + 1;
                    }
                }
            }
            var newRs = new ResultSet();
            newRs.Load(newTable);
            return newRs;
        }
    }
}
