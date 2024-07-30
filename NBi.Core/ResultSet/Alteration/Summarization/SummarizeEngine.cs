using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Extensibility;
using NBi.Core.Sequence.Transformation.Aggregation;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    class SummarizeEngine : ISummarizationEngine
    {
        private SummarizeArgs Args { get; }

        public SummarizeEngine(SummarizeArgs args)
            => Args = args;

        public IResultSet Execute(IResultSet rs)
        {
            using var dataTable = new DataTableResultSet();
            foreach (var groupBy in Args.GroupBys)
            {
                var column = groupBy.Identifier.GetColumn(rs) ?? throw new NullReferenceException();
                dataTable.AddColumn(column.Name, column.DataType);
            }

            var factory = new AggregationFactory();
            var aggregations = new List<Aggregation>();
            foreach (var aggregation in Args.Aggregations)
            {
                var columnName = ExtractColumnName(rs, aggregation);

                dataTable.AddColumn(columnName, MapType(aggregation.Function, aggregation.ColumnType));
                aggregations.Add(factory.Instantiate(aggregation));
            }

            var groupbyFactory = new GroupByFactory();
            var groupbyEngine = groupbyFactory.Instantiate(new ColumnGroupByArgs(Args.GroupBys, Context.None));
            var groups = groupbyEngine.Execute(rs);
            foreach (var group in groups)
            {
                var values = new List<object>();
                values.AddRange(group.Key.Members);
                foreach (var aggregation in aggregations.Zip(Args.Aggregations, (x, y) => new { Implementation = x, Definition = y }))
                {
                    var inputs = new List<object>();
                    foreach (var groupRow in group.Value.Rows)
                        if (aggregation.Definition.Identifier == null)
                            inputs.Add(1);
                        else
                            inputs.Add(groupRow.GetValue(aggregation.Definition.Identifier) ?? throw new NullReferenceException());

                    var aggrResult = aggregation.Implementation.Execute(inputs);
                    values.Add(aggrResult ?? throw new NullReferenceException());
                }
                dataTable.AddRow([.. values]);
            }
            dataTable.AcceptChanges();
            return dataTable;
        }

        private string ExtractColumnName(IResultSet rs, ColumnAggregationArgs aggregation)
        {
            if (aggregation.Identifier == null)
                return "count";

            var column = aggregation.Identifier.GetColumn(rs) ?? throw new NullReferenceException();

            var columnName = Args.Aggregations.Count(x => (x.Identifier.GetColumn(rs) ?? throw new NullReferenceException()).Equals(new ColumnNameIdentifier(column.Name).GetColumn(rs))) > 1
                ? $"{column.Name}_{aggregation.Function}"
                : column.Name;
            return columnName;
        }

        private Type MapType(AggregationFunctionType function, ColumnType type)
        {
            return type switch
            {
                ColumnType.Text => typeof(string),
                ColumnType.Numeric => typeof(decimal),
                ColumnType.DateTime => typeof(DateTime),
                ColumnType.Boolean => typeof(bool),
                _ => throw new ArgumentException(),
            };
        }
    }
}
