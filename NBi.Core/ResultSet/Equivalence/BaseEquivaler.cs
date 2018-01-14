using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using System.Text;
using NBi.Core.ResultSet.Caster;
using NBi.Core.ResultSet.Analyzer;
using System.Collections.ObjectModel;

namespace NBi.Core.ResultSet.Equivalence
{
    public abstract class BaseEquivaler : IEquivaler
    {
        private readonly IList<IRowsAnalyzer> analyzers;
        private IReadOnlyCollection<IRowsAnalyzer> Analyzers
        {
            get { return new ReadOnlyCollection<IRowsAnalyzer>(analyzers); }
        }

        private readonly CellComparer cellComparer = new CellComparer();
        protected CellComparer CellComparer
        {
            get { return cellComparer; }
        }

        public BaseEquivaler(IEnumerable<IRowsAnalyzer> analyzers)
        {
            this.analyzers = new List<IRowsAnalyzer>(analyzers);
        }

        public ISettingsResultSet Settings { get; set; }

        public abstract EngineStyle Style { get; }
        

        private readonly Dictionary<KeyCollection, RowHelper> xDict = new Dictionary<KeyCollection, RowHelper>();
        private readonly Dictionary<KeyCollection, RowHelper> yDict = new Dictionary<KeyCollection, RowHelper>();

        
        public ResultResultSet Compare(object x, object y)
        {
            if (x is DataTable && y is DataTable)
                return doCompare((DataTable)y, (DataTable)x);

            if (x is ResultSet && y is ResultSet)
                return doCompare(((ResultSet)y).Table, ((ResultSet)x).Table);

            throw new ArgumentException();
        }

        protected virtual ResultResultSet doCompare(DataTable x, DataTable y)
        {
            var stopWatch = new Stopwatch();

            var columnsCount = Math.Max(y.Columns.Count, x.Columns.Count);

            PreliminaryChecks(x, y);

            var keyComparer = BuildDataRowsKeyComparer(x);

            stopWatch.Start();
            BuildRowDictionary(x, xDict, keyComparer, false);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Building first rows dictionary: {0} [{1}]", x.Rows.Count, stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            stopWatch.Reset();

            stopWatch.Start();
            BuildRowDictionary(y, yDict, keyComparer, true);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Building second rows dictionary: {0} [{1}]", y.Rows.Count, stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            stopWatch.Reset();

            var missingRowsAnalyzer = analyzers.FirstOrDefault(a => a.GetType() == typeof(MissingRowsAnalyzer));
            var missingRows = missingRowsAnalyzer?.Retrieve(xDict, yDict) ?? new List<RowHelper>();

            var unexpectedRowsAnalyzer = analyzers.FirstOrDefault(a => a.GetType() == typeof(UnexpectedRowsAnalyzer));
            var unexpectedRows = unexpectedRowsAnalyzer?.Retrieve(xDict, yDict) ?? new List<RowHelper>();

            var keyMatchingRowsAnalyzer = analyzers.FirstOrDefault(a => a.GetType() == typeof(KeyMatchingRowsAnalyzer));
            var keyMatchingRows = keyMatchingRowsAnalyzer?.Retrieve(xDict, yDict) ?? new List<RowHelper>();

            stopWatch.Start();
            var nonMatchingValueRows = !CanSkipValueComparison() ? CompareSets(keyMatchingRows) : new List<DataRow>();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Rows with a matching key but without matching value: {0} [{1}]", nonMatchingValueRows.Count(), stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")));
            stopWatch.Reset();

            var duplicatedRows = new List<DataRow>(); // Dummy placeholder

            return ResultResultSet.Build(
                missingRows.Select(a => a.DataRowObj).ToList(),
                unexpectedRows.Select(a => a.DataRowObj).ToList(),
                duplicatedRows,
                keyMatchingRows.Select(a => a.DataRowObj).ToList(),
                nonMatchingValueRows
                );
        }

        protected abstract void PreliminaryChecks(DataTable x, DataTable y);
        protected abstract DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x);
        //protected abstract void CompareValues(List<CompareHelper> keyMatchingRows, List<DataRow> nonMatchingValueRows);
        protected virtual bool CanSkipValueComparison()
        {
            return false;
        }

        protected List<DataRow> CompareSets(List<RowHelper> keyMatchingRows)
        {
            var stopWatch = new Stopwatch();
            int i = 0;

            var nonMatchingValueRows = new List<DataRow>();
            foreach (var rxHelper in keyMatchingRows)
            {
                i++;
                stopWatch.Restart();
                var ryHelper = yDict[rxHelper.Keys];
                var rx = rxHelper.DataRowObj;
                var ry = ryHelper.DataRowObj;

                var nonMatchingValueRow = CompareRows(rx, ry);
                if (nonMatchingValueRow != null)
                    nonMatchingValueRows.Add(nonMatchingValueRow);

                if (i==1)
                    Trace.WriteLineIf(
                        NBiTraceSwitch.TraceInfo,
                        $"Comparison of first row: [{stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}]"
                        );
            }
            return nonMatchingValueRows;
        }

        protected abstract DataRow CompareRows(DataRow x, DataRow y);

        

        private void BuildRowDictionary(DataTable dt, Dictionary<KeyCollection, RowHelper> dict, DataRowKeysComparer keyComparer, bool isSystemUnderTest)
        {
            dict.Clear();
            foreach (DataRow row in dt.Rows)
            {
                RowHelper hlpr = new RowHelper();

                var keys = keyComparer.GetKeys(row);

                hlpr.Keys = keys;
                hlpr.DataRowObj = row;

                //Check that the rows in the reference are unique
                // All the rows should be unique regardless of whether it is the system under test or the result set.
                if (dict.ContainsKey(keys))
                {
                    throw new EquivalerException(
                        string.Format("The {0} data set has some duplicated keys. Check your keys definition or the result set defined in your {1}. The duplicated hashcode is {2}.\r\nRow to insert:{3}.\r\nRow already inserted:{4}.",
                            isSystemUnderTest ? "actual" : "expected",
                            isSystemUnderTest ? "system-under-test" : "assertion",
                            keys.GetHashCode(),
                            RowToString(row),
                            RowToString(dict[keys].DataRowObj)
                            )
                        );
                }

                dict.Add(keys, hlpr);
            }
        }

        private string RowToString(DataRow row)
        {
            var sb = new StringBuilder();
            sb.Append("<");
            foreach (var obj in row.ItemArray)
            {
                if (obj == null)
                    sb.Append("(null)");
                else
                    sb.Append(obj.ToString());
                sb.Append("|");
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            sb.Append(">");

            return sb.ToString();
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

                    var numericCaster = new NumericCaster();
                    if (columnType == ColumnType.Numeric && !(numericCaster.IsValid(value) || Comparer.BaseComparer.IsValidInterval(value)))
                    {
                        var exception = string.Format(messages[0]
                            , columnName, value.ToString());

                        if (numericCaster.IsValid(value.ToString().Replace(",", ".")))
                            exception += messages[1];

                        throw new EquivalerException(exception);
                    }

                    if (columnType == ColumnType.DateTime && IsDateTimeField(dataColumn))
                        return;

                    if (columnType == ColumnType.DateTime && !Comparer.BaseComparer.IsValidDateTime(value.ToString()))
                    {
                        throw new EquivalerException(
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
