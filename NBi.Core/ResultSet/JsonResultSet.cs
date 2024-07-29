using NBi.Extensibility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class JsonResultSet
    {
        public IResultSet Build(string json)
        {
            var dt = (DataTable)(JsonConvert.DeserializeObject(json, typeof(DataTable)) ?? throw new NullReferenceException());

            var isArrayConverted = false;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var column = dt.Columns[i];
                if (column.DataType == typeof(DataTable))
                {
                    if (isArrayConverted)
                        throw new InvalidOperationException("NBi can't convert a JSON document with more than one array to a result-set.");

                    isArrayConverted = true;
                    var newColumns = new List<DataColumn>();
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((row.ItemArray[column.Ordinal]) is DataTable rowTable) //could be the case if the array is empty
                            foreach (DataColumn subColumn in rowTable.Columns)
                            {
                                var columnName = string.Format("{0}.{1}", column.ColumnName, subColumn.ColumnName);
                                if (!newColumns.Exists(c => c.ColumnName == columnName))
                                {
                                    var newColumn = new DataColumn()
                                    {
                                        ColumnName = columnName,
                                        DataType = subColumn.DataType,
                                        AllowDBNull = true,
                                        DefaultValue = DBNull.Value
                                    };
                                    newColumn.ExtendedProperties.Add("split", subColumn.ColumnName);
                                    newColumns.Add(newColumn);
                                }
                            }
                    }

                    var j = 1;
                    foreach (var newColumn in newColumns)
                    {
                        dt.Columns.Add(newColumn);
                        newColumn.SetOrdinal(column.Ordinal + j);
                        j++;
                    }

                    var k = 0;
                    while(k<dt.Rows.Count)
                    {
                        var masterRow = dt.Rows[k];
                        var subTable = (masterRow.ItemArray[column.Ordinal]) as DataTable;
                        var l = 1;
                        foreach (DataRow rowTable in subTable!.Rows)
                        {
                            var newRow = dt.NewRow();
                            foreach (DataColumn columnIter in dt.Columns)
                            {
                                if (column != columnIter && !columnIter.ExtendedProperties.ContainsKey("split"))
                                {
                                    newRow[columnIter.Ordinal] = masterRow[columnIter.Ordinal];
                                }
                                else if (column != columnIter && columnIter.ExtendedProperties.ContainsKey("split"))
                                {
                                    var columnName = (string)columnIter.ExtendedProperties["split"]!;
                                    if (subTable.Columns.Contains(columnName))
                                        newRow[columnIter.Ordinal] = rowTable[columnName];
                                }
                            }
                            dt.Rows.InsertAt(newRow, k+l);
                            l++;
                        }
                        if (l>1) //If the array is empty we don't need to remove the row!
                            dt.Rows.RemoveAt(k);

                        k += l;
                    }

                    dt.Columns.Remove(column);

                    foreach (DataColumn columnIter in dt.Columns)
                    {
                        if (columnIter.ExtendedProperties.ContainsKey("split"))
                            columnIter.ExtendedProperties.Remove("split");
                    }
                }
            }

            return new DataTableResultSet(dt);
        }
    }
}
