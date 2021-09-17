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
            using (var dataTable = new DataTableResultSet())
            {
                foreach (var groupBy in Args.GroupBys)
                {
                    var column = groupBy.Identifier.GetColumn(rs);
                    dataTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
                }

                var factory = new AggregationFactory();
                var aggregations = new List<Aggregation>();
                foreach (var aggregation in Args.Aggregations)
                {
                    var columnName = ExtractColumnName(rs, aggregation);

                    dataTable.Columns.Add(new DataColumn(columnName, MapType(aggregation.Function, aggregation.ColumnType)));
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
                                inputs.Add(groupRow.GetValue(aggregation.Definition.Identifier));

                        var aggrResult = aggregation.Implementation.Execute(inputs);
                        values.Add(aggrResult);
                    }

                    var row = dataTable.NewRow();
                    row.ItemArray = values.ToArray();
                    dataTable.Add(row);
                }
                dataTable.AcceptChanges();
                return dataTable;
            }
        }

        private string ExtractColumnName(IResultSet rs, ColumnAggregationArgs aggregation)
        {
            if (aggregation.Identifier == null)
                return "count";

            var column = aggregation.Identifier.GetColumn(rs);

            var columnName = Args.Aggregations.Count(x => x.Identifier.GetColumn(rs) == new ColumnNameIdentifier(column.ColumnName).GetColumn(rs)) > 1
                ? $"{column.ColumnName}_{aggregation.Function}"
                : column.ColumnName;
            return columnName;
        }

        private Type MapType(AggregationFunctionType function, ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Text: return typeof(string);
                case ColumnType.Numeric: return typeof(decimal);
                case ColumnType.DateTime: return typeof(DateTime);
                case ColumnType.Boolean: return typeof(bool);
                default: throw new ArgumentException();
            }
        }
    }
}
