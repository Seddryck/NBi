using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Extensibility;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Reshaping
{
    class UnstackEngine : IReshapingEngine
    {
        private UnstackArgs Args { get; }

        public UnstackEngine(UnstackArgs args)
            => Args = args;

        public IResultSet Execute(IResultSet rs)
        {
            //Build structre of the resulting table
            var valueColumns = rs.Columns.Where(
                    x => !x.Equals(Args.Header.GetColumn(rs))
                    && !Args.GroupBys.Select(y => y.Identifier.GetColumn(rs)).Contains(x)
                );

            var headerValues = Args.EnforcedColumns.Select(x => x.Name).ToList();
            foreach (var row in rs.Rows)
                headerValues.Add(Args.Header.GetValue(row)?.ToString() ?? "null");
            headerValues = headerValues.Distinct().ToList();

            using (var dataTable = new DataTableResultSet())
            {
                foreach (var groupBy in Args.GroupBys)
                {
                    var column = groupBy.Identifier.GetColumn(rs) ?? throw new NullReferenceException();
                    dataTable.AddColumn(column.Name, column.DataType);
                }

                var namingStrategy = (valueColumns.Count() == 1)
                    ? new UniqueValueNamingStrategy() as INamingStrategy
                    : new MultipleValuesNamingStrategy();

                foreach (var valueColumn in valueColumns)
                    foreach (var headerValue in headerValues)
                        dataTable.AddColumn(namingStrategy.Execute(headerValue, valueColumn.Name));


                var groupbyFactory = new GroupByFactory();
                var groupbyEngine = groupbyFactory.Instantiate(new ColumnGroupByArgs(Args.GroupBys, Context.None));
                var groups = groupbyEngine.Execute(rs);
                foreach (var group in groups)
                {
                    var newRow = dataTable.NewRow();
                    var itemArray = newRow.ItemArray;
                    new List<object>(group.Key.Members).CopyTo(0, itemArray!, 0, group.Key.Members.Count());


                    var alreadyValued = new List<string>();
                    foreach (var groupRow in group.Value.Rows)
                        foreach (var valueColumn in valueColumns)
                        {
                            var nameValueColumn = namingStrategy.Execute(groupRow[Args.Header]?.ToString() ?? string.Empty, valueColumn.Name);
                            if (!alreadyValued.Contains(nameValueColumn))
                            {
                                alreadyValued.Add(nameValueColumn);
                                var identifier = new ColumnNameIdentifier(nameValueColumn);
                                var col = dataTable.GetColumn(identifier);
                                if (col is null)
                                    throw new Exception($"Unexpected Column {identifier.Name}");
                                var colOrdinal = col.Ordinal;
                                itemArray[colOrdinal] = groupRow[valueColumn.Name];
                            }
                            else
                                throw new ArgumentException();
                        }
                    newRow.ItemArray = itemArray;
                    dataTable.AddRow(newRow);
                }
                dataTable.AcceptChanges();
                return dataTable;
            }
        }

        private interface INamingStrategy
        {
            string Execute(string header, string value);
        }

        private class UniqueValueNamingStrategy : INamingStrategy
        {
            public string Execute(string header, string value) => header;
        }

        private class MultipleValuesNamingStrategy : INamingStrategy
        {
            public string Execute(string header, string value) => $"{header}_{value}";
        }
    }
}
