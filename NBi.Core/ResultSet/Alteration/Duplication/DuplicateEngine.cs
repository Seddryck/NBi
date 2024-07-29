using NBi.Core.Calculation.Asserting;
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
            var result = rs.Clone();
            result.Clear();

            //Add the new columns
            foreach (var output in Outputs)
            {
                if (result.GetColumn(output.Identifier) == null)
                {
                    switch (output.Identifier)
                    {
                        case ColumnNameIdentifier identifier:
                            result.AddColumn(identifier.Name);
                            break;
                        case ColumnOrdinalIdentifier identifier:
                            result.AddColumn($"Column_{identifier.Ordinal}");
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (var row in rs.Rows)
            {
                Context.Switch(row);
                var isDuplicated = Predication.Execute(Context);
                var times = Times.Execute();

                var importedRow = result.AddRow(row);
                foreach (var output in Outputs)
                {
                    if (output.Strategy?.IsApplicable(true) ?? false)
                    {
                        var columnName = result.GetColumn(output.Identifier)?.Name ?? throw new InvalidOperationException();
                        importedRow[columnName] = output.Strategy.Execute(true, isDuplicated, times, 0);
                    }
                }

                if (isDuplicated)
                {
                    for (int i = 0; i < times; i++)
                    {
                        Context.Switch(importedRow);
                        var duplicatedRow = result.AddRow(importedRow);
                        foreach (var output in Outputs)
                        {
                            if (output.Strategy?.IsApplicable(false) ?? false)
                            {
                                var columnName = result.GetColumn(output.Identifier)?.Name ?? throw new InvalidOperationException();
                                duplicatedRow[columnName] = output.Strategy.Execute(false, true, times, i);
                                Context.Switch(duplicatedRow);
                            }
                        }
                    }
                }
            }
            result.AcceptChanges();
            return result;
        }
    }
}
