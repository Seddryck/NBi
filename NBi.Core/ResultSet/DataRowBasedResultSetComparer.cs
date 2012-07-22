using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowBasedResultSetComparer : IResultSetComparer
    {
        public ResultSetComparaisonSettings Settings { get; set; }

        public DataRowBasedResultSetComparer()
        {
            Settings = new ResultSetComparaisonSettings(0);
        }

        public DataRowBasedResultSetComparer(ResultSetComparaisonSettings settings)
        {
            Settings = settings;
        }
        
        public int Compare(object x, object y)
        {
            if (x is DataTable && y is DataTable)
                return doCompare((DataTable)y, (DataTable)x);

            throw new ArgumentException();
        }

        protected int doCompare(DataTable x, DataTable y)
        {
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
                            if (!IsEqual(Convert.ToDouble(rx[i]), Convert.ToDouble(ry[i]), Convert.ToDouble(Settings.Tolerances(i))))
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

            return Convert.ToInt32(!(missingRows.Count() == 0 && unexpectedRows.Count() == 0 && nonMatchingValueRows.Count()==0));
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

    }
}
