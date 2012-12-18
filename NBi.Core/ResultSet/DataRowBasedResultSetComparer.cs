using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowBasedResultSetComparer : IResultSetComparer
    {
        public ResultSetComparisonSettings Settings { get; set; }

        public DataRowBasedResultSetComparer()
        {
        }

        public DataRowBasedResultSetComparer(ResultSetComparisonSettings settings)
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
                BuildDefaultSettings();

            Settings.ConsoleDisplay();
            WriteSettingsToDataTableProperties(y, Settings);
            WriteSettingsToDataTableProperties(x, Settings);
            
            var keyComparer = new DataRowKeysComparer(Settings, x.Columns.Count);

            var missingRows = x.AsEnumerable().Except(y.AsEnumerable(), keyComparer);
            Console.WriteLine("Missing rows: {0}", missingRows.Count());

            var unexpectedRows = y.AsEnumerable().Except(x.AsEnumerable(),keyComparer);
            Console.WriteLine("Unexpected rows: {0}", unexpectedRows.Count());

            var keyMatchingRows = x.AsEnumerable().Except(missingRows).Except(unexpectedRows);
            Console.WriteLine("Rows with a key matching: {0}", keyMatchingRows.Count());

            var nonMatchingValueRows = new List<DataRow>(); 
            foreach (var rx in keyMatchingRows)
	        {
                var ry = y.AsEnumerable().Single(r => keyComparer.GetHashCode(r) == keyComparer.GetHashCode(rx));
                for (int i = 0; i < rx.Table.Columns.Count; i++)
                {
                    if (Settings.IsValue(i))
                    {
                        //Null management
                        if (rx.IsNull(i) || ry.IsNull(i))
                        {
                             if (!rx.IsNull(i) || !ry.IsNull(i))
                             {
                                 ry.SetColumnError(i, ry.IsNull(i) ? rx[i].ToString() : "(null)");
                                 if (!nonMatchingValueRows.Contains(ry))
                                     nonMatchingValueRows.Add(ry);
                             }
                        }
                        //Not Null management
                        else
                        {
                            //Numeric
                            if (Settings.IsNumeric(i))
                            {
                                //Console.WriteLine("Debug: {0} {1}", rx[i].ToString(), rx[i].GetType());

                                //Convert to decimal
                                var rxDecimal = Convert.ToDecimal(rx[i], NumberFormatInfo.InvariantInfo);
                                var ryDecimal = Convert.ToDecimal(ry[i], NumberFormatInfo.InvariantInfo);
                                var tolerance = Convert.ToDecimal(Settings.GetTolerance(i), NumberFormatInfo.InvariantInfo);
                                
                                //Compare decimals (with tolerance)
                                if (!IsEqual(rxDecimal, ryDecimal, tolerance))
                                {
                                    ry.SetColumnError(i, rxDecimal.ToString());
                                    if (!nonMatchingValueRows.Contains(ry))
                                        nonMatchingValueRows.Add(ry);
                                }
                                
                            }
                            //Not Numeric
                            else
                            {
                                if (!IsEqual(rx[i], ry[i]))
                                {
                                    ry.SetColumnError(i, rx[i].ToString());
                                    if (!nonMatchingValueRows.Contains(ry))
                                        nonMatchingValueRows.Add(ry);
                                }
                            }
                        }
                    }
                }
	        }
            Console.WriteLine("Rows with a key matching but without value matching: {0}", nonMatchingValueRows.Count());

            return ResultSetCompareResult.Build(missingRows, unexpectedRows, keyMatchingRows, nonMatchingValueRows);
        }

        protected void WriteSettingsToDataTableProperties(DataTable dt, ResultSetComparisonSettings settings)
        {
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ExtendedProperties.ContainsKey("NBi::Role"))
                    column.ExtendedProperties["NBi::Role"] = settings.GetColumnRole(column.Ordinal);
                else
                    column.ExtendedProperties.Add("NBi::Role", settings.GetColumnRole(column.Ordinal));

                if (column.ExtendedProperties.ContainsKey("NBi::Type"))
                    column.ExtendedProperties["NBi::Type"] = settings.GetColumnType(column.Ordinal);
                else
                    column.ExtendedProperties.Add("NBi::Type", settings.GetColumnType(column.Ordinal));

                if (column.ExtendedProperties.ContainsKey("NBi::Tolerance"))
                    column.ExtendedProperties["NBi::Tolerance"] = settings.GetTolerance(column.Ordinal);
                else
                    column.ExtendedProperties.Add("NBi::Tolerance", settings.GetTolerance(column.Ordinal));
            }
        }

        protected internal bool IsEqual(Decimal x, Decimal y, Decimal tolerance)
        {
            //Console.WriteLine("IsEqual: {0} {1} {2} {3} {4} {5}", x, y, tolerance, Math.Abs(x - y), x == y, Math.Abs(x - y) <= tolerance);

            //quick check
            if (x == y)
                return true;

            //Stop checks if tolerance is set to 0
            if (tolerance == 0)
                return false;

            //include some math[Time consumming] (Tolerance needed to validate)
            return (Math.Abs(x - y) <= tolerance);
        }

        private bool IsEqual(object x, object y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        protected void BuildDefaultSettings()
        {
            Settings = new ResultSetComparisonSettings(
                ResultSetComparisonSettings.KeysChoice.AllExpectLast, 
                ResultSetComparisonSettings.ValuesChoice.Last, 
                null);
        }

    }
}
