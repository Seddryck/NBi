using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Analyzer;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Equivalence
{
    internal class NameEquivaler : BaseEquivaler
    {
        public override EngineStyle Style
            => EngineStyle.ByName;

        private new SettingsNameResultSet Settings
            => (SettingsNameResultSet)base.Settings!;

        public NameEquivaler(IEnumerable<IRowsAnalyzer> analyzers, SettingsNameResultSet settings)
            : base(analyzers, settings)
        { }

        protected override void PreliminaryChecks(IResultSet x, IResultSet y)
        {
            RemoveIgnoredColumns(y, Settings ?? throw new InvalidOperationException());
            RemoveIgnoredColumns(x, Settings);

            WriteSettingsToDataTableProperties(y, Settings);
            WriteSettingsToDataTableProperties(x, Settings);

            CheckSettingsAndDataTable(y, Settings);
            CheckSettingsAndDataTable(x, Settings);

            CheckSettingsAndFirstRow(y, Settings);
            CheckSettingsAndFirstRow(x, Settings);
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x)
            => new DataRowKeysComparerByName(Settings);

        protected override IResultRow? CompareRows(IResultRow rx, IResultRow ry)
        {
            var isRowOnError = false;
            foreach (var columnName in Settings.GetValueNames())
            {
                var x = rx.IsNull(columnName) ? DBNull.Value : rx[columnName];
                var y = ry.IsNull(columnName) ? DBNull.Value : ry[columnName];
                var rounding = Settings.IsRounding(columnName) ? Settings.GetRounding(columnName) : null;
                var result = CellComparer.Compare(x!, y!, Settings.GetColumnType(columnName), Settings.GetTolerance(columnName), rounding);

                if (!result.AreEqual)
                {
                    ry.SetColumnError(columnName, result.Message);
                    if (!isRowOnError)
                        isRowOnError = true;
                }
            }
            if (isRowOnError)
                return ry;
            else
                return null;
        }


        protected void RemoveIgnoredColumns(IResultSet dt, SettingsNameResultSet settings)
        {
            var i = 0;
            while (i < dt.ColumnCount)
            {
                var column = dt.GetColumn(i) ?? throw new InvalidOperationException();
                if (settings.GetColumnRole(column.Name) == ColumnRole.Ignore)
                    column.Remove();
                else
                    i++;
            }
        }

        protected void WriteSettingsToDataTableProperties(IResultSet dt, SettingsNameResultSet settings)
        {
            foreach (var column in dt.Columns)
            {
                column.SetProperties(
                    settings.GetColumnRole(column.Name)
                    , settings.GetColumnType(column.Name)
                    , settings.GetTolerance(column.Name)
                    , settings.GetRounding(column.Name)
                );
            }
        }

        protected void CheckSettingsAndDataTable(IResultSet dt, SettingsNameResultSet settings)
        {
            var missingColumns = new List<KeyValuePair<string,string>>();
            foreach (var columnName in settings.GetKeyNames())
            {
                if (!dt.ContainsColumn(columnName))
                    missingColumns.Add(new KeyValuePair<string, string>(columnName, "key"));
            }

            foreach (var columnName in settings.GetValueNames())
            {
                if (!dt.ContainsColumn(columnName))
                    missingColumns.Add(new KeyValuePair<string, string>(columnName, "value"));
            }

            if (missingColumns.Count > 0)
            {
                var exception = string.Format("You've defined {0} column{1} named '{2}' as key{1} or value{1} but there is no column with {3} name{1} in the resultset. When using comparison by columns' name, you must ensure that all columns defined as keys and values are effectively available in the result-set."
                    , missingColumns.Count > 1 ? "some" : "a"
                    , missingColumns.Count > 1 ? "s" : string.Empty
                    , string.Join("', '", missingColumns.Select(kv => kv.Key))
                    , missingColumns.Count > 1 ? "these" : "this"
                    );

                throw new EquivalerException(exception);
            }
        }

        protected void CheckSettingsAndFirstRow(IResultSet dt, SettingsNameResultSet settings)
        {
            if (dt.RowCount == 0)
                return;

            var dr = dt[0];
            for (int i = 0; i < dt.ColumnCount; i++)
            {
                var columnName = dt.GetColumn(i)?.Name ?? throw new InvalidOperationException();
                CheckSettingsFirstRowCell(
                        settings.GetColumnRole(columnName)
                        , settings.GetColumnType(columnName)
                        , dt.GetColumn(columnName) ?? throw new InvalidOperationException()
                        , dr.IsNull(columnName) ? DBNull.Value : dr[columnName] ?? throw new InvalidOperationException()
                        ,
                            [
                                "The column named '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                                , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                                , "The column named '{0}' is expecting a date & time value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                            ]
                );
            }
        }
    }
}
