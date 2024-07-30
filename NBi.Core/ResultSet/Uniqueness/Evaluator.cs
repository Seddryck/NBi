using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using System.Text;
using NBi.Core.Scalar.Casting;
using NBi.Core.ResultSet.Analyzer;
using System.Collections.ObjectModel;
using NBi.Core.ResultSet.Equivalence;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Uniqueness
{
    public abstract class Evaluator
    {
        protected ISettingsResultSet Settings { get; set; }

        private readonly CellComparer cellComparer = new ();
        protected CellComparer CellComparer
            => cellComparer;

        
        public Evaluator(ISettingsResultSet settings)
            => Settings = settings;

        private readonly Dictionary<KeyCollection, int> dict = [];

        public ResultUniqueRows Execute(IResultSet x)
        {
            var stopWatch = new Stopwatch();

            var columnsCount = x.ColumnCount;

            PreliminaryChecks(x);

            var keyComparer = BuildDataRowsKeyComparer(x);

            stopWatch.Start();
            BuildRowDictionary(x, keyComparer, dict);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Building dictionary: {x.RowCount} [{stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]");
            stopWatch.Reset();

            var duplicatedRows = dict.Where(r => r.Value > 1);
            
            return new ResultUniqueRows(
                    x.RowCount
                    , duplicatedRows
                );
        }

        protected abstract void PreliminaryChecks(IResultSet x);
        protected abstract DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x);
        
        private void BuildRowDictionary(IResultSet rs, DataRowKeysComparer keyComparer, Dictionary<KeyCollection, int> dict)
        {
            dict.Clear();
            foreach (var row in rs.Rows)
            {
                RowHelper hlpr = new RowHelper();

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
        
        protected bool IsNumericField(IResultColumn dataColumn)
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

        protected bool IsDateTimeField(IResultColumn dataColumn)
            => dataColumn.DataType == typeof(DateTime);


        protected void CheckSettingsFirstRowCell(ColumnRole columnRole, ColumnType columnType, IResultColumn dataColumn, object value, string[] messages)
        {
            var columnName = dataColumn.Name;
            if (!DBNull.Value.Equals(value))
            {
                if (columnRole != ColumnRole.Ignore)
                {
                    if (columnType == ColumnType.Numeric && IsNumericField(dataColumn))
                        return;

                    var numericConverter = new NumericCaster();
                    if (columnType == ColumnType.Numeric && !(numericConverter.IsValid(value) || BaseComparer.IsValidInterval(value)))
                    {
                        var exception = string.Format(messages[0]
                            , columnName, value.ToString());

                        if (numericConverter.IsValid((value.ToString() ?? string.Empty).Replace(",", ".")))
                            exception += messages[1];

                        throw new EquivalerException(exception);
                    }

                    if (columnType == ColumnType.DateTime && IsDateTimeField(dataColumn))
                        return;

                    if (columnType == ColumnType.DateTime && !BaseComparer.IsValidDateTime(value.ToString() ?? string.Empty))
                    {
                        throw new EquivalerException(
                            string.Format(messages[2]
                                , columnName, value.ToString()));
                    }
                }
            }
        }
    }
}
