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
    internal class ResultSetComparerByName : ResultSetComparer
    {
        public override ComparisonStyle Style
        {
            get
            {
                return ComparisonStyle.ByName;
            }
        }

        private SettingsResultSetComparisonByName settings
        {
            get { return Settings as SettingsResultSetComparisonByName; }
        }

        public ResultSetComparerByName(SettingsResultSetComparisonByName settings)
        {
            Settings = settings;
        }

        protected override void PreliminaryChecks(DataTable x, DataTable y)
        {
            if (Settings == null)
                throw new InvalidOperationException();

            RemoveIgnoredRows(y, settings);
            RemoveIgnoredRows(x, settings);

            WriteSettingsToDataTableProperties(y, settings);
            WriteSettingsToDataTableProperties(x, settings);

            CheckSettingsAndDataTable(y, settings);
            CheckSettingsAndDataTable(x, settings);

            CheckSettingsAndFirstRow(y, settings);
            CheckSettingsAndFirstRow(x, settings);
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
        {
            return new DataRowKeysComparerByName(settings);
        }

        protected override DataRow CompareRows(DataRow rx, DataRow ry)
        {
            var isRowOnError = false;
            foreach (var columnName in settings.GetValueNames())
            {
                var x = rx.IsNull(columnName) ? DBNull.Value : rx[columnName];
                var y = ry.IsNull(columnName) ? DBNull.Value : ry[columnName];
                var rounding = settings.IsRounding(columnName) ? settings.GetRounding(columnName) : null;
                var result = base.CellComparer.Compare(x, y, settings.GetColumnType(columnName), settings.GetTolerance(columnName), rounding);

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


        protected void RemoveIgnoredRows(DataTable dt, SettingsResultSetComparisonByName settings)
        {
            var i = 0;
            while (i < dt.Columns.Count)
            {
                if (settings.GetColumnRole(dt.Columns[i].ColumnName) == ColumnRole.Ignore)
                    dt.Columns.RemoveAt(i);
                else
                    i++;
            }
        }

        protected void WriteSettingsToDataTableProperties(DataTable dt, SettingsResultSetComparisonByName settings)
        {
            foreach (DataColumn column in dt.Columns)
            {
                WriteSettingsToDataTableProperties(
                    column
                    , settings.GetColumnRole(column.ColumnName)
                    , settings.GetColumnType(column.ColumnName)
                    , settings.GetTolerance(column.ColumnName)
                    , settings.GetRounding(column.ColumnName)
                );
            }
        }


        protected void CheckSettingsAndDataTable(DataTable dt, SettingsResultSetComparisonByName settings)
        {
            var missingColumns = new List<KeyValuePair<string,string>>();
            foreach (var columnName in settings.GetKeyNames())
            {
                if (!dt.Columns.Contains(columnName))
                    missingColumns.Add(new KeyValuePair<string, string>(columnName, "key"));
            }

            foreach (var columnName in settings.GetValueNames())
            {
                if (!dt.Columns.Contains(columnName))
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

                throw new ResultSetComparerException(exception);
            }
        }

        protected void CheckSettingsAndFirstRow(DataTable dt, SettingsResultSetComparisonByName settings)
        {
            if (dt.Rows.Count == 0)
                return;

            var dr = dt.Rows[0];
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                var columnName = dr.Table.Columns[i].ColumnName;
                CheckSettingsFirstRowCell(
                        settings.GetColumnRole(columnName)
                        , settings.GetColumnType(columnName)
                        , dr.Table.Columns[columnName]
                        , dr.IsNull(columnName) ? DBNull.Value : dr[columnName]
                        , new string[]
                            {
                                "The column named '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                                , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                                , "The column named '{0}' is expecting a date & time value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                            }
                );
            }
        }
    }
}
