using NBi.Core.Calculation.Grouping;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Transformation.Aggregation;
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

        public ResultSet Execute(ResultSet rs)
        {
            //Build structre of the resulting table
            var valueColumns = rs.Columns.Cast<DataColumn>().Where(
                    x => x != Args.Header.GetColumn(rs.Table)
                    && !Args.GroupBys.Select(y => y.Identifier.GetColumn(rs.Table)).Contains(x)
                );
            

            var headerValues = Args.EnforcedColumns.Select(x => x.Name).ToList();
            rs.Rows.Cast<DataRow>().ToList().ForEach(x => headerValues.Add(Args.Header.GetValue(x).ToString()));
            headerValues = headerValues.Distinct().ToList();

            using (var dataTable = new DataTable())
            {
                foreach (var groupBy in Args.GroupBys)
                {
                    var column = groupBy.Identifier.GetColumn(rs.Table);
                    dataTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
                }

                var namingStrategy = (valueColumns.Count() == 1)
                    ? new UniqueValueNamingStrategy() as INamingStrategy
                    : new MultipleValuesNamingStrategy();

                foreach (var valueColumn in valueColumns)
                    foreach (var headerValue in headerValues)
                        dataTable.Columns.Add(new DataColumn(namingStrategy.Execute(headerValue, valueColumn.ColumnName), typeof(object)));


                var groupbyFactory = new GroupByFactory();
                var groupbyEngine = groupbyFactory.Instantiate(Args.GroupBys);
                var groups = groupbyEngine.Execute(rs);
                foreach (var group in groups)
                {
                    var newRow = dataTable.NewRow();
                    var itemArray = newRow.ItemArray;
                    new List<object>(group.Key.Members).CopyTo(0, itemArray, 0, group.Key.Members.Count());


                    var alreadyValued = new List<string>();
                    foreach (DataRow groupRow in group.Value.Rows)
                        foreach (var valueColumn in valueColumns)
                        {
                            var nameValueColumn = namingStrategy.Execute(groupRow.GetValue(Args.Header).ToString(), valueColumn.ColumnName);
                            if (!alreadyValued.Contains(nameValueColumn))
                            {
                                alreadyValued.Add(nameValueColumn);
                                var identifier = new ColumnNameIdentifier(nameValueColumn);
                                itemArray[dataTable.GetColumn(identifier).Ordinal] = groupRow[valueColumn.ColumnName];
                            }
                            else
                                throw new ArgumentException();
                        }
                    newRow.ItemArray = itemArray;
                    dataTable.Rows.Add(newRow);
                }
                dataTable.AcceptChanges();
                rs.Load(dataTable);
            }
            return rs;
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
