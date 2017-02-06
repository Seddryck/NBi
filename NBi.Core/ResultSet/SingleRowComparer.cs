using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using System.Text;
using NBi.Core.ResultSet.Converter;

namespace NBi.Core.ResultSet
{
    public class SingleRowComparer : IResultSetComparer
    {
        public IResultSetComparisonSettings Settings
        {
            get { return indexBasedSettings; }
            set
            {
                if (!(value is ResultSetComparisonByIndexSettings))
                    throw new ArgumentException();
                indexBasedSettings = value as ResultSetComparisonByIndexSettings;
            }
        }

        private ResultSetComparisonByIndexSettings indexBasedSettings;

        private readonly NumericComparer numericComparer = new NumericComparer();
        private readonly TextComparer textComparer = new TextComparer();
        private readonly DateTimeComparer dateTimeComparer = new DateTimeComparer();
        private readonly BooleanComparer booleanComparer = new BooleanComparer();

        public SingleRowComparer()
        {
        }

        public SingleRowComparer(ResultSetComparisonByIndexSettings settings)
        {
            indexBasedSettings = settings;
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
            if (x.Rows.Count > 1)
                throw new ArgumentException(string.Format("The query in the assertion returns {0} rows. It was expected to return zero or one row.", x.Rows.Count));

            if (y.Rows.Count > 1)
                throw new ArgumentException(string.Format("The query in the system-under-test returns {0} rows. It was expected to return zero or one row.", y.Rows.Count));

            return doCompare(x.Rows.Count == 1 ? x.Rows[0] : null, y.Rows.Count == 1 ? y.Rows[0] : null);
        }

        protected ResultSetCompareResult doCompare(DataRow x, DataRow y)
        {
            var chrono = DateTime.Now;

            var missingRows = new List<DataRow>();
            var unexpectedRows = new List<DataRow>();

            if (x == null && y != null)
                unexpectedRows.Add(y);

            if (x != null && y == null)
                missingRows.Add(x);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

            IEnumerable<DataRow> nonMatchingValueRows = null;
            if (missingRows.Count == 0 && unexpectedRows.Count == 0)
            {
                chrono = DateTime.Now;
                var columnsCount = Math.Max(y.Table.Columns.Count, x.Table.Columns.Count);
                if (indexBasedSettings == null)
                    BuildDefaultSettings(columnsCount);
                else
                    indexBasedSettings.ApplyTo(columnsCount);

                WriteSettingsToDataTableProperties(y.Table, indexBasedSettings);
                WriteSettingsToDataTableProperties(x.Table, indexBasedSettings);

                CheckSettingsAndDataTable(y.Table, indexBasedSettings);
                CheckSettingsAndDataTable(x.Table, indexBasedSettings);

                CheckSettingsAndFirstRow(y.Table, indexBasedSettings);
                CheckSettingsAndFirstRow(x.Table, indexBasedSettings);
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Analyzing length and format of result-sets: [{0}]", DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));

                // If all of the columns make up the key, then we already know which rows match and which don't.
                //  So there is no need to continue testing
                chrono = DateTime.Now;
                nonMatchingValueRows = CompareValues(x, y);
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key but without matching value: {0} [{1}]", nonMatchingValueRows.Count(), DateTime.Now.Subtract(chrono).ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            }

            return ResultSetCompareResult.Build(
                missingRows,
                unexpectedRows,
                new List<DataRow>(),
                new List<DataRow>(),
                nonMatchingValueRows ?? new List<DataRow>()
                );
        }

        private IEnumerable<DataRow> CompareValues(DataRow rx, DataRow ry)
        {
            var nonMatchingValueRows = new List<DataRow>();
            var isRowOnError = false;

            for (int i = 0; i < rx.Table.Columns.Count; i++)
            {
                if (indexBasedSettings.GetColumnRole(i) == ColumnRole.Value)
                {
                    //Null management
                    if (rx.IsNull(i) || ry.IsNull(i))
                    {
                        if (!rx.IsNull(i) || !ry.IsNull(i))
                        {
                            ry.SetColumnError(i, ry.IsNull(i) ? rx[i].ToString() : "(null)");
                            if (!isRowOnError)
                            {
                                isRowOnError = true;
                                nonMatchingValueRows.Add(ry);
                            }

                        }
                    }
                    //(value) management
                    else if (rx[i].ToString() == "(value)" || ry[i].ToString() == "(value)")
                    {
                        if (rx.IsNull(i) || ry.IsNull(i))
                        {
                            ry.SetColumnError(i, rx[i].ToString());
                            if (!isRowOnError)
                            {
                                isRowOnError = true;
                                nonMatchingValueRows.Add(ry);
                            }
                        }
                    }
                    //Not Null management
                    else
                    {
                        ComparerResult result = null;

                        //Numeric
                        if (indexBasedSettings.GetColumnType(i) == ColumnType.Numeric)
                        {
                            //Convert to decimal
                            if (indexBasedSettings.IsRounding(i))
                                result = numericComparer.Compare(rx[i], ry[i], indexBasedSettings.GetRounding(i));
                            else
                                result = numericComparer.Compare(rx[i], ry[i], indexBasedSettings.GetTolerance(i));
                        }
                        //Date and Time
                        else if (indexBasedSettings.GetColumnType(i) == ColumnType.DateTime)
                        {
                            //Convert to dateTime
                            if (indexBasedSettings.IsRounding(i))
                                result = dateTimeComparer.Compare(rx[i], ry[i], indexBasedSettings.GetRounding(i));
                            else
                                result = dateTimeComparer.Compare(rx[i], ry[i], indexBasedSettings.GetTolerance(i));
                        }
                        //Boolean
                        else if (indexBasedSettings.GetColumnType(i) == ColumnType.Boolean)
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
                            if (!isRowOnError)
                            {
                                isRowOnError = true;
                                nonMatchingValueRows.Add(ry);
                            }
                        }
                    }
                }
            }
            return nonMatchingValueRows;
        }

        protected void WriteSettingsToDataTableProperties(DataTable dt, ResultSetComparisonByIndexSettings settings)
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

                if (column.ExtendedProperties.ContainsKey("NBi::Rounding"))
                    column.ExtendedProperties["NBi::Rounding"] = settings.GetRounding(column.Ordinal);
                else
                    column.ExtendedProperties.Add("NBi::Rounding", settings.GetRounding(column.Ordinal));
            }
        }


        protected void CheckSettingsAndDataTable(DataTable dt, ResultSetComparisonByIndexSettings settings)
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

        protected void CheckSettingsAndFirstRow(DataTable dt, ResultSetComparisonByIndexSettings settings)
        {
            if (dt.Rows.Count == 0)
                return;

            var dr = dt.Rows[0];
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                if (!dr.IsNull(i))
                {
                    if (settings.GetColumnType(i) == ColumnType.Numeric && IsNumericField(dr.Table.Columns[i]))
                        continue;

                    var numericConverter = new NumericConverter();
                    if (settings.GetColumnType(i) == ColumnType.Numeric && !(numericConverter.IsValid(dr[i]) || BaseComparer.IsValidInterval(dr[i])))
                    {
                        var exception = string.Format("The column with an index of {0} is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                            , i, dr[i].ToString());

                        if (numericConverter.IsValid(dr[i].ToString().Replace(",", ".")))
                            exception += " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'.";

                        throw new ResultSetComparerException(exception);
                    }

                    if (settings.GetColumnType(i) == ColumnType.DateTime && IsDateTimeField(dr.Table.Columns[i]))
                        return;

                    if (settings.GetColumnType(i) == ColumnType.DateTime && !BaseComparer.IsValidDateTime(dr[i].ToString()))
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
            indexBasedSettings = new SingleRowComparisonSettings();
        }

    }
}
