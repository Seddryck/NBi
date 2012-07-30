using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowBasedResultSetComparer : IResultSetComparer
    {
        public ResultSetComparaisonSettings Settings { get; set; }

        public DataRowBasedResultSetComparer()
        {
        }

        public DataRowBasedResultSetComparer(ResultSetComparaisonSettings settings)
        {
            Settings = settings;
        }
        
        public ResultSetCompareResult Compare(object x, object y)
        {
            if (x is DataTable && y is DataTable)
                return doCompare((DataTable)y, (DataTable)x);

            if (x is ResultSet && y is ResultSet)
                return doCompare(((ResultSet)y).Table, ((ResultSet)x).Table);

            throw new ArgumentException();
        }

        protected ResultSetCompareResult doCompare(DataTable x, DataTable y)
        {
            if (Settings == null)
                BuildDefaultSettings(x.Columns);
            
            var KeyComparer = new DataRowKeysComparer(Settings.KeyColumnIndexes);

            var missingRows = x.AsEnumerable().Except(y.AsEnumerable(), KeyComparer);

            var unexpectedRows = y.AsEnumerable().Except(x.AsEnumerable(),KeyComparer);

            var keyMatchingRows = x.AsEnumerable().Except(missingRows).Except(unexpectedRows);

            var nonMatchingValueRows = new List<DataRow>(); 
            foreach (var rx in keyMatchingRows)
	        {
		        var ry = y.AsEnumerable().Single( r => (new DataRowKeysComparer()).GetHashCode(r) == (new DataRowKeysComparer()).GetHashCode(rx));
                for (int i = 0; i < rx.Table.Columns.Count; i++)
                {
                    if (IsValueColumn(i))
                    {
                        if (IsNumericColumn(i))
                        {
                            if (!IsEqual(Convert.ToDouble(rx[i], NumberFormatInfo.InvariantInfo)
                                , Convert.ToDouble(ry[i], NumberFormatInfo.InvariantInfo)
                                , Convert.ToDouble(Settings.Tolerances(i), NumberFormatInfo.InvariantInfo)))
                                nonMatchingValueRows.Add(ry);
                        }
                        else
                        {
                            if (!IsEqual(rx[i], ry[i]))
                                nonMatchingValueRows.Add(ry);
                        }
                    }
                }
	        }

            return ResultSetCompareResult.Build(missingRows, unexpectedRows, keyMatchingRows, nonMatchingValueRows);
        }

        private bool IsValueColumn(int index)
        {
            return Settings.ValueColumnIndexes.Contains(index);
        }

        private bool IsNumericColumn(int index)
        {
            return (index >= 1);
        }

        private bool IsEqual(double x, double y, double tolerance)
        {
            return (Math.Abs(x - y) <= tolerance);
        }

        private bool IsEqual(object x, object y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        protected void BuildDefaultSettings(DataColumnCollection columns)
        {
            Settings = new ResultSetComparaisonSettings(columns.Count, 0, 0);
        }

    }
}
