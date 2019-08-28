using NBi.Core.Calculation.Grouping;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    class SummerizeEngine : ISummarizationEngine
    {
        private SummarizeArgs Args { get; }

        public SummerizeEngine(SummarizeArgs args)
            => Args = args;

        public ResultSet Execute(ResultSet rs)
        {
            var dataTable = new DataTable();
            foreach (var groupBy in Args.GroupBys)
            {
                var column = groupBy.Identifier.GetColumn(rs.Table);
                dataTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
            }

            var factory = new AggregationFactory();
            var aggregations = new List<Aggregation>();
            foreach (var aggregation in Args.Aggregations)
            {
                var column = aggregation.Identifier.GetColumn(rs.Table);

                var columnName = Args.Aggregations.Count(x => x.Identifier.GetColumn(rs.Table) == new ColumnNameIdentifier(column.ColumnName).GetColumn(rs.Table)) > 1
                    ? $"{column.ColumnName}_{aggregation.Function.ToString()}"
                    : column.ColumnName;

                dataTable.Columns.Add(new DataColumn(columnName, MapType(aggregation.Function, aggregation.ColumnType)));
                aggregations.Add(factory.Instantiate(aggregation));
            }

            var groupbyFactory = new ByColumnGroupingFactory();
            var groupbyEngine = groupbyFactory.Instantiate(Args.GroupBys);
            var groups = groupbyEngine.Execute(rs);
            foreach (var group in groups)
            {
                var values = new List<object>();
                values.AddRange(group.Key.Members);
                foreach (var aggregation in aggregations.Zip(Args.Aggregations, (x, y) => new { Implementation = x, Definition = y }))
                {
                    var inputs = new List<object>();
                    foreach (DataRow groupRow in group.Value.Rows)
                        inputs.Add(groupRow.GetValue(aggregation.Definition.Identifier));

                    var aggrResult = aggregation.Implementation.Execute(inputs);
                    values.Add(aggrResult);
                }

                var row = dataTable.NewRow();
                row.ItemArray = values.ToArray();
                dataTable.Rows.Add(row);
            }
            dataTable.AcceptChanges();
            rs.Load(dataTable);
            return rs;
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
