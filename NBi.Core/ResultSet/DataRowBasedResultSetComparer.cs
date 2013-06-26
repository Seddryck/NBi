using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet
{
    public class DataRowBasedResultSetComparer : IResultSetComparer
    {
        public ResultSetComparisonSettings Settings { get; set; }

        private readonly Dictionary<Int64, CompareHelper> xDict = new Dictionary<long, CompareHelper>();
        private readonly Dictionary<Int64, CompareHelper> yDict = new Dictionary<long, CompareHelper>();

        private readonly BaseComparer baseComparer = new BaseComparer();
        private readonly NumericComparer numericComparer = new NumericComparer();
        private readonly TextComparer textComparer = new TextComparer();
        private readonly DateTimeComparer dateTimeComparer = new DateTimeComparer();
        private readonly BooleanComparer booleanComparer = new BooleanComparer();

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
                        string.Format("The {0} data set has some duplicated keys. Check your keys definition or the result set defined in your {1}.", 
                            IsSystemUnderTest ? "actual" : "expected",
                            IsSystemUnderTest ? "system-under-test" : "assertion"
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
                            //(value) management
                            else if (rx[i].ToString() == "(value)" || ry[i].ToString() == "(value)")
                            {
                                if (rx.IsNull(i) || ry.IsNull(i))
                                {
                                    ry.SetColumnError(i, rx[i].ToString());
                                    if (!nonMatchingValueRows.Contains(ry))
                                        nonMatchingValueRows.Add(ry);
                                }
                            }
                            //Not Null management
                            else
                            {
                                ComparerResult result = null;

                                //Numeric
                                if (Settings.IsNumeric(i))
                                {
                                    //Convert to decimal
                                    result = numericComparer.Compare(rx[i], ry[i], Settings.GetTolerance(i));
                                }
                                //Date and Time
                                else if (Settings.IsDateTime(i))
                                {
                                    //Convert to dateTime
                                    result = dateTimeComparer.Compare(rx[i], ry[i]);
                                }
                                //Boolean
                                else if (Settings.IsBoolean(i))
                                {
                                    //Convert to bool
                                    result = booleanComparer.Compare(rx[i], ry[i]);
                                }
                                //Text
                                else
                                {
                                    result = textComparer.Compare(rx[i], ry[i]);
                                }

                                //If are not equal then we need to set the message in the ColumnError.
                                if (!result.AreEqual)
                                {
                                    ry.SetColumnError(i, result.Message);
                                    if (!nonMatchingValueRows.Contains(ry))
                                        nonMatchingValueRows.Add(ry);
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

                    if (settings.IsNumeric(i) && !baseComparer.IsValidNumeric(dr[i]))
                    {                   
                        var exception = string.Format("The column with an index of {0} is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value."
                            , i, dr[i].ToString());

                        if (baseComparer.IsValidNumeric(dr[i].ToString().Replace(",", ".")))
                            exception += " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'.";

                        throw new ResultSetComparerException(exception);
                    }

                    if (settings.IsDateTime(i) && IsDateTimeField(dr.Table.Columns[i]))
                        return;

                    if (settings.IsDateTime(i) && !baseComparer.IsValidDateTime(dr[i].ToString()))
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

        protected void BuildDefaultSettings(int columnsCount)
        {
            Settings = new ResultSetComparisonSettings(
                columnsCount,
                ResultSetComparisonSettings.KeysChoice.AllExpectLast,
                ResultSetComparisonSettings.ValuesChoice.Last);
        }

    }
}
