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

            Settings.ConsoleDisplay();
            
            var KeyComparer = new DataRowKeysComparer(Settings, x.Columns.Count);

            var missingRows = x.AsEnumerable().Except(y.AsEnumerable(), KeyComparer);
            Console.WriteLine("Missing rows: {0}", missingRows.Count());

            var unexpectedRows = y.AsEnumerable().Except(x.AsEnumerable(),KeyComparer);
            //Console.WriteLine("Unexpected rows: {0}", unexpectedRows.Count());

            var keyMatchingRows = x.AsEnumerable().Except(missingRows).Except(unexpectedRows);
            Console.WriteLine("Rows with a key matching: {0}", keyMatchingRows.Count());

            var nonMatchingValueRows = new List<DataRow>(); 
            foreach (var rx in keyMatchingRows)
	        {
                var ry = y.AsEnumerable().Single(r => KeyComparer.GetHashCode(r) == KeyComparer.GetHashCode(rx));
                for (int i = 0; i < rx.Table.Columns.Count; i++)
                {
                    if (Settings.IsValue(i))
                    {
                        //Null management
                        if (rx.IsNull(i) || ry.IsNull(i))
                        {
                             if (!rx.IsNull(i) || !ry.IsNull(i))
                                 nonMatchingValueRows.Add(ry);
                        }
                        //Not Null management
                        else
                        {
                            //Numeric
                            if (Settings.IsNumeric(i))
                            {
                                //Convert to decimal
                                Console.WriteLine("Debug: {0} {1}", rx[i].ToString(), rx[i].GetType());
                                var rxDecimal = Convert.ToDecimal(rx[i], NumberFormatInfo.InvariantInfo);
                                var ryDecimal = Convert.ToDecimal(ry[i], NumberFormatInfo.InvariantInfo);
                                var tolerance = Convert.ToDecimal(Settings.GetTolerance(i), NumberFormatInfo.InvariantInfo);
                                
                                //Compare decimals (with tolerance)
                                if (!IsEqual(rxDecimal, ryDecimal, tolerance))
                                    nonMatchingValueRows.Add(ry);
                                
                            }
                            //Not Numeric
                            else
                            {
                                if (!IsEqual(rx[i], ry[i]))
                                    nonMatchingValueRows.Add(ry);
                            }
                        }
                    }
                }
	        }
            Console.WriteLine("Rows with a key matching but without value matching: {0}", nonMatchingValueRows.Count());

            return ResultSetCompareResult.Build(missingRows, unexpectedRows, keyMatchingRows, nonMatchingValueRows);
        }

        protected internal bool IsEqual(Decimal x, Decimal y, Decimal tolerance)
        {
            Console.WriteLine("IsEqual: {0} {1} {2} {3} {4} {5}", x, y, tolerance, Math.Abs(x - y), x == y, Math.Abs(x - y) <= tolerance);
            
            return (Math.Abs(x - y) <= tolerance);
        }

        private bool IsEqual(object x, object y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        protected void BuildDefaultSettings(DataColumnCollection columns)
        {
            Settings = new ResultSetComparaisonSettings(
                ResultSetComparaisonSettings.KeysChoice.AllExpectLast, 
                ResultSetComparaisonSettings.ValuesChoice.Last, 
                null);
        }

    }
}
