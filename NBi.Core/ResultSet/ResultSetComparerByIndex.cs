using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using System.Text;
using NBi.Core.ResultSet.Converter;
using NBi.Core.ResultSet;

namespace NBi.Core.ResultSet
{
    public class ResultSetComparerByIndex : ResultSetComparer
    {
        private SettingsResultSetComparisonByIndex settings
        {
            get { return Settings as SettingsResultSetComparisonByIndex; }
        }

        public ResultSetComparerByIndex(SettingsResultSetComparisonByIndex settings)
        {
            Settings = settings;
        }

        protected override void PreliminaryChecks(DataTable x, DataTable y)
        {
            var columnsCount = Math.Max(y.Columns.Count, x.Columns.Count);
            if (settings == null)
                BuildDefaultSettings(columnsCount);
            else
                settings.ApplyTo(columnsCount);

            WriteSettingsToDataTableProperties(y, settings);
            WriteSettingsToDataTableProperties(x, settings);

            CheckSettingsAndDataTable(y, settings);
            CheckSettingsAndDataTable(x, settings);

            CheckSettingsAndFirstRow(y, settings);
            CheckSettingsAndFirstRow(x, settings);
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
        {
            return new DataRowKeysComparerByIndex(settings, x.Columns.Count);
        }

        protected override bool CanSkipValueComparison()
        {
            return settings.KeysDef == SettingsResultSetComparisonByIndex.KeysChoice.All;
        }


        protected override DataRow CompareRows(DataRow rx, DataRow ry)
        {
            var isRowOnError = false;
            for (int i = 0; i < rx.Table.Columns.Count; i++)
            {
                if (settings.GetColumnRole(i) == ColumnRole.Value)
                {
                    var x = rx.IsNull(i) ? DBNull.Value : rx[i];
                    var y = ry.IsNull(i) ? DBNull.Value : ry[i];
                    var rounding = settings.IsRounding(i) ? settings.GetRounding(i) : null;
                    var result = CellComparer.Compare(x, y, settings.GetColumnType(i), settings.GetTolerance(i), rounding);

                    if (!result.AreEqual)
                    {
                        ry.SetColumnError(i, result.Message);
                        if (!isRowOnError)
                            isRowOnError = true;
                    }
                }
            }
            if (isRowOnError)
                return ry;
            else
                return null;
        }

        protected void WriteSettingsToDataTableProperties(DataTable dt, SettingsResultSetComparisonByIndex settings)
        {
            foreach (DataColumn column in dt.Columns)
            {
                WriteSettingsToDataTableProperties(
                    column
                    , settings.GetColumnRole(column.Ordinal)
                    , settings.GetColumnType(column.Ordinal)
                    , settings.GetTolerance(column.Ordinal)
                    , settings.GetRounding(column.Ordinal)
                );
            }
        }

        protected void CheckSettingsAndDataTable(DataTable dt, SettingsResultSetComparisonByIndex settings)
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

        protected void CheckSettingsAndFirstRow(DataTable dt, SettingsResultSetComparisonByIndex settings)
        {
            if (dt.Rows.Count == 0)
                return;

            var dr = dt.Rows[0];
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                CheckSettingsFirstRowCell(
                        settings.GetColumnRole(i)
                        , settings.GetColumnType(i)
                        , dr.Table.Columns[i]
                        , dr.IsNull(i) ? DBNull.Value : dr[i]
                        , new string[]
                            {
                                "The column with index '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                                , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                                , "The column with index '{0}' is expecting a 'date & time' value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                            }
                );
            }
        }

        protected virtual void BuildDefaultSettings(int columnsCount)
        {
            Settings = new SettingsResultSetComparisonByIndex(
                columnsCount,
                SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                SettingsResultSetComparisonByIndex.ValuesChoice.Last);
        }

    }
}
