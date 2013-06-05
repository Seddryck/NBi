using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowBasedResultSetComparer : IResultSetComparer
    {
        public ResultSetComparisonSettings Settings { get; set; }

        Dictionary<Int64, CompareHelper> xDict = new Dictionary<long, CompareHelper>();
        Dictionary<Int64, CompareHelper> yDict = new Dictionary<long, CompareHelper>();

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

        private void CalculateHashValues(DataTable dt, Dictionary<Int64, CompareHelper> dict, DataRowKeysComparer keyComparer, bool IsSystemUnderTest)
        {
            dict.Clear();

            Int64 keysHashed;
            Int64 valuesHashed;

            foreach (DataRow row in dt.Rows)
            {
                CompareHelper hlpr = new CompareHelper();

                keyComparer.GetHashCode64_KeysValues(row, out keysHashed, out valuesHashed);
                
                hlpr.KeysHashed = keysHashed;
                hlpr.ValuesHashed = valuesHashed;
                hlpr.DataRowObj = row;

                //Check that the rows in the reference are unique
                // All the rows should be unique regardless of whether it is the system under test or the result set.
                if (dict.ContainsKey(keysHashed))
                {
                    throw new ResultSetComparerException(
                        string.Format("The {0} data set has some duplicated keys. Check your keys definition or your expected result set.", 
                            IsSystemUnderTest ? "system under test" : "results"
                            )
                        );
                }

                dict.Add(keysHashed, hlpr);
            }
        }

        protected ResultSetCompareResult doCompare(DataTable x, DataTable y)
        {
            var chrono = DateTime.Now;

            var columnsCount = Math.Max(y.Columns.Count, x.Columns.Count);
            if (Settings == null)
                BuildDefaultSettings(columnsCount);
            else
                Settings.ApplyTo(columnsCount);

            Settings.ConsoleDisplay();
            WriteSettingsToDataTableProperties(y, Settings);
            WriteSettingsToDataTableProperties(x, Settings);

            CheckSettingsAndDataTable(y, Settings);
            CheckSettingsAndDataTable(x, Settings);

            CheckSettingsAndFirstRow(y, Settings);
            CheckSettingsAndFirstRow(x, Settings);

            var keyComparer = new DataRowKeysComparer(Settings, x.Columns.Count);

            CalculateHashValues(x, xDict, keyComparer, true);
            CalculateHashValues(y, yDict, keyComparer, false);

            chrono = DateTime.Now;
            List<CompareHelper> missingRows;
            {
                var missingRowKeys = xDict.Keys.Except(yDict.Keys);
                missingRows = new List<CompareHelper>(missingRowKeys.Count());
                foreach (Int64 i in missingRowKeys)
                {
                    missingRows.Add(xDict[i]);
                }
            }
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Missing rows: {0} [{1}]", missingRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            chrono = DateTime.Now;
            List<CompareHelper> unexpectedRows;
            {
                var unexpectedRowKeys = yDict.Keys.Except(xDict.Keys);
                unexpectedRows = new List<CompareHelper>(unexpectedRowKeys.Count());
                foreach (Int64 i in unexpectedRowKeys)
                {
                    unexpectedRows.Add(yDict[i]);
                }
            }
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Unexpected rows: {0} [{1}]", unexpectedRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            chrono = DateTime.Now;
            List<CompareHelper> keyMatchingRows;
            {
                var keyMatchingRowKeys = xDict.Keys.Intersect(yDict.Keys);
                keyMatchingRows = new List<CompareHelper>(keyMatchingRowKeys.Count());
                foreach (Int64 i in keyMatchingRowKeys)
                {
                    keyMatchingRows.Add(xDict[i]);
                }
            }
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key and not duplicated: {0}  [{1}]", keyMatchingRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            chrono = DateTime.Now;
            var nonMatchingValueRows = new List<DataRow>();

            // If all of the columns make up the key, then we already know which rows match and which don't.
            //  So there is no need to continue testing
            if (Settings.KeysDef != ResultSetComparisonSettings.KeysChoice.All)
            {
                foreach (var rxHelper in keyMatchingRows)
                {
                    var ryHelper = yDict[rxHelper.KeysHashed];

                    if (ryHelper.ValuesHashed == rxHelper.ValuesHashed)
                    {
                        // quick shortcut. If the hash of the values matches, then there is no further need to test
                        continue;
                    }

                    var rx = rxHelper.DataRowObj;
                    var ry = ryHelper.DataRowObj;

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
                                //Date and Time
                                else if (Settings.IsDateTime(i))
                                {
                                    //Console.WriteLine("Debug: {0} {1}", rx[i].ToString(), rx[i].GetType());

                                    //Convert to decimal
                                    var rxDateTime = Convert.ToDateTime(rx[i], DateTimeFormatInfo.InvariantInfo);
                                    var ryDateTime = Convert.ToDateTime(ry[i], DateTimeFormatInfo.InvariantInfo);

                                    //Compare decimals (with tolerance)
                                    if (!IsEqual(rxDateTime, ryDateTime))
                                    {
                                        ry.SetColumnError(i, rxDateTime.ToString());
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
            }
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key but without matching value: {0} [{1}]", nonMatchingValueRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            var duplicatedRows = new List<DataRow>(); // Dummy place holder

            return ResultSetCompareResult.Build(
                missingRows.Select(a => a.DataRowObj).ToList(),
                unexpectedRows.Select(a => a.DataRowObj).ToList(),
                duplicatedRows,
                keyMatchingRows.Select(a => a.DataRowObj).ToList(),
                nonMatchingValueRows
                );
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


        protected void CheckSettingsAndDataTable(DataTable dt, ResultSetComparisonSettings settings)
        {
            var max = settings.GetMaxColumnIndexDefined();
            if (dt.Columns.Count <= max)
            {
                var exception = string.Format("You've defined a column with an index of {0}, meaning that your result set would have at least {1} columns but your result set has only {2} columns."
                    , max
                    , max + 1
                    , dt.Columns.Count);

                if (dt.Columns.Count == max && settings.GetMinColumnIndexDefined() == 1)
                    exception += " You've no definition for a column with an index of 0. Are you sure you'vent started to index at 1 in place of 0?";

                throw new ResultSetComparerException(exception);
            }
        }

        protected void CheckSettingsAndFirstRow(DataTable dt, ResultSetComparisonSettings settings)
        {
            if (dt.Rows.Count == 0)
                return;

            var dr = dt.Rows[0];
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                if (!dr.IsNull(i))
                {
                    if (settings.IsNumeric(i) && IsNumericField(dr.Table.Columns[i]))
                        continue;

                    if (settings.IsNumeric(i) && !IsValidNumeric(dr[i]))
                    {                   
                        var exception = string.Format("The column with an index of {0} is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value."
                            , i, dr[i].ToString());
                            
                        if (IsValidNumeric(dr[i].ToString().Replace(",", ".")))
                            exception += " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'.";

                        throw new ResultSetComparerException(exception);
                    }

                    if (settings.IsDateTime(i) && IsDateTimeField(dr.Table.Columns[i]))
                        return;

                    if (settings.IsDateTime(i) && !IsValidDateTime(dr[i].ToString()))
                    {
                        throw new ResultSetComparerException(
                            string.Format("The column with an index of {0} is expecting a date & time value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                                , i, dr[i].ToString()));
                    } 
                }
            }
        }

        private bool IsNumericField(DataColumn dataColumn)
        {
            return
                dataColumn.DataType == typeof(Byte) ||
                dataColumn.DataType == typeof(Decimal) ||
                dataColumn.DataType == typeof(Double) ||
                dataColumn.DataType == typeof(Int16) ||
                dataColumn.DataType == typeof(Int32) ||
                dataColumn.DataType == typeof(Int64) ||
                dataColumn.DataType == typeof(SByte) ||
                dataColumn.DataType == typeof(Single) ||
                dataColumn.DataType == typeof(UInt16) ||
                dataColumn.DataType == typeof(UInt32) ||
                dataColumn.DataType == typeof(UInt64);
        }

        private bool IsDateTimeField(DataColumn dataColumn)
        {
            return
                dataColumn.DataType == typeof(DateTime);
        }

        private bool IsValidNumeric(object value)
        {
            decimal num = 0;
            var result =  Decimal.TryParse(value.ToString()
                                , NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint
                                , CultureInfo.InvariantCulture
                                , out num);
            //The first method is not enough, you can have cases where this method returns false but the value is effectively a numeric. The problem is in the .ToString() on the object where you apply the regional settings for the numeric values.
            //The second method gives a better result but unfortunately generates an exception.
            if (!result)
            {
                try
                {
                    num = Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
                    result = true;
                }
                catch (Exception)
                {

                    result = false;
                }
            }
            return result;
        }

        private bool IsValidDateTime(string value)
        {
            DateTime dateTime = DateTime.MinValue;
            return DateTime.TryParse(value
                                , CultureInfo.InvariantCulture.DateTimeFormat
                                , DateTimeStyles.AllowWhiteSpaces
                                , out dateTime);
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

        protected void BuildDefaultSettings(int columnsCount)
        {
            Settings = new ResultSetComparisonSettings(
                columnsCount,
                ResultSetComparisonSettings.KeysChoice.AllExpectLast,
                ResultSetComparisonSettings.ValuesChoice.Last);
        }

    }
}
