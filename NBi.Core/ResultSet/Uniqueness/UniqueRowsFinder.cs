using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using System.Text;
using NBi.Core.ResultSet.Converter;
using NBi.Core.ResultSet.Analyzer;
using System.Collections.ObjectModel;

namespace NBi.Core.ResultSet.Uniqueness
{
    public abstract class UniqueRowsFinder
    {
        private readonly CellComparer cellComparer = new CellComparer();
        protected CellComparer CellComparer
        {
            get { return cellComparer; }
        }

        public UniqueRowsFinder()
        {
        }

        public ISettingsResultSetComparison Settings { get; set; }

        private readonly Dictionary<KeyCollection, int> dict = new Dictionary<KeyCollection, int>();

        public UniqueRowsResult Execute(object x)
        {
            if (x is DataTable)
                return doCompare((DataTable)x);

            if (x is ResultSet)
                return doCompare(((ResultSet)x).Table);

            throw new ArgumentException();
        }

        protected virtual UniqueRowsResult doCompare(DataTable x)
        {
            var stopWatch = new Stopwatch();

            var columnsCount = x.Columns.Count;

            PreliminaryChecks(x);

            var keyComparer = BuildDataRowsKeyComparer(x);

            stopWatch.Start();
            BuildRowDictionary(x, keyComparer, dict);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Building dictionary: {0} [{1}]", x.Rows.Count, stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            stopWatch.Reset();

            var duplicatedRows = dict.Where(r => r.Value > 1);
            
            return new UniqueRowsResult(
                    x.Rows.Count
                    , duplicatedRows
                );
        }

        protected abstract void PreliminaryChecks(DataTable x);
        protected abstract DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x);
        
        private void BuildRowDictionary(DataTable dt, DataRowKeysComparer keyComparer, Dictionary<KeyCollection, int> dict)
        {
            dict.Clear();
            foreach (DataRow row in dt.Rows)
            {
                CompareHelper hlpr = new CompareHelper();

                var keys = keyComparer.GetKeys(row);

                hlpr.Keys = keys;

                //Check that the rows in the reference are unique
                // All the rows should be unique regardless of whether it is the system under test or the result set.
                if (dict.ContainsKey(keys))
                    dict[keys]++;
                else
                    dict.Add(keys, 1);
            }
        }
        
        protected bool IsNumericField(DataColumn dataColumn)
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

        protected bool IsDateTimeField(DataColumn dataColumn)
        {
            return
                dataColumn.DataType == typeof(DateTime);
        }


        protected void CheckSettingsFirstRowCell(ColumnRole columnRole, ColumnType columnType, DataColumn dataColumn, object value, string[] messages)
        {
            var columnName = dataColumn.ColumnName;
            if (!DBNull.Value.Equals(value))
            {
                if (columnRole != ColumnRole.Ignore)
                {
                    if (columnType == ColumnType.Numeric && IsNumericField(dataColumn))
                        return;

                    var numericConverter = new NumericConverter();
                    if (columnType == ColumnType.Numeric && !(numericConverter.IsValid(value) || BaseComparer.IsValidInterval(value)))
                    {
                        var exception = string.Format(messages[0]
                            , columnName, value.ToString());

                        if (numericConverter.IsValid(value.ToString().Replace(",", ".")))
                            exception += messages[1];

                        throw new ResultSetComparerException(exception);
                    }

                    if (columnType == ColumnType.DateTime && IsDateTimeField(dataColumn))
                        return;

                    if (columnType == ColumnType.DateTime && !BaseComparer.IsValidDateTime(value.ToString()))
                    {
                        throw new ResultSetComparerException(
                            string.Format(messages[2]
                                , columnName, value.ToString()));
                    }
                }
            }
        }

        protected void WriteSettingsToDataTableProperties(DataColumn column, ColumnRole role, ColumnType type, Tolerance tolerance, Rounding rounding)
        {
            if (column.ExtendedProperties.ContainsKey("NBi::Role"))
                column.ExtendedProperties["NBi::Role"] = role;
            else
                column.ExtendedProperties.Add("NBi::Role", role);

            if (column.ExtendedProperties.ContainsKey("NBi::Type"))
                column.ExtendedProperties["NBi::Type"] = type;
            else
                column.ExtendedProperties.Add("NBi::Type", type);

            if (column.ExtendedProperties.ContainsKey("NBi::Tolerance"))
                column.ExtendedProperties["NBi::Tolerance"] = tolerance;
            else
                column.ExtendedProperties.Add("NBi::Tolerance", tolerance);

            if (column.ExtendedProperties.ContainsKey("NBi::Rounding"))
                column.ExtendedProperties["NBi::Rounding"] = rounding;
            else
                column.ExtendedProperties.Add("NBi::Rounding", rounding);
        }
    }
}
