using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using System.Text;
using NBi.Core.Scalar.Casting;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Equivalence;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class OrdinalEvaluator : Evaluator
    {
        protected new SettingsOrdinalResultSet Settings
        {
            get => (SettingsOrdinalResultSet)base.Settings;
            set => base.Settings = value;
        }
        
        public OrdinalEvaluator()
            : base(new UnsetSettings())
        { }

        public OrdinalEvaluator(SettingsOrdinalResultSet settings)
            : base(settings)
        { }

        protected override void PreliminaryChecks(IResultSet x)
        {
            var columnsCount = x.ColumnCount;
            if (Settings is UnsetSettings)
                Settings = BuildDefaultSettings(columnsCount);
            else
                Settings.ApplyTo(columnsCount);

            WriteSettingsToDataTableProperties(x, Settings);
            CheckSettingsAndDataTable(x, Settings);
            CheckSettingsAndFirstRow(x, Settings);
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x)
            => new DataRowKeysComparerByOrdinal(Settings, x.ColumnCount);

        protected void WriteSettingsToDataTableProperties(IResultSet dt, SettingsOrdinalResultSet settings)
        {
            foreach (var column in dt.Columns)
            {
                column.SetProperties(
                    settings.GetColumnRole(column.Ordinal)
                    , settings.GetColumnType(column.Ordinal)
                );
            }
        }

        protected void CheckSettingsAndDataTable(IResultSet dt, SettingsOrdinalResultSet settings)
        {
            var max = settings.GetMaxColumnOrdinalDefined();
            if (dt.ColumnCount <= max)
            {
                var exception = string.Format("You've defined a column with an index of {0}, meaning that your result set would have at least {1} columns but your result set has only {2} columns."
                    , max
                    , max + 1
                    , dt.ColumnCount);

                if (dt.ColumnCount == max && settings.GetMinColumnOrdinalDefined() == 1)
                    exception += " You've no definition for a column with an index of 0. Are you sure you'vent started to index at 1 in place of 0?";

                throw new EquivalerException(exception);
            }
        }

        protected void CheckSettingsAndFirstRow(IResultSet dt, SettingsOrdinalResultSet settings)
        {
            if (dt.RowCount == 0)
                return;

            var dr = dt[0];
            for (int i = 0; i < dt.ColumnCount; i++)
            {
                CheckSettingsFirstRowCell(
                        settings.GetColumnRole(i)
                        , settings.GetColumnType(i)
                        , dt.GetColumn(i) ?? throw new NullReferenceException()
                        , dr.IsNull(i) ? DBNull.Value : dr[i] ?? throw new NullReferenceException()
                        ,
                            [
                                "The column with index '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                                , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                                , "The column with index '{0}' is expecting a 'date & time' value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                            ]
                );
            }
        }

        protected static SettingsOrdinalResultSet BuildDefaultSettings(int columnsCount)
            => new SettingsOrdinalResultSet(
                columnsCount,
                SettingsOrdinalResultSet.KeysChoice.All,
                SettingsOrdinalResultSet.ValuesChoice.None);

        private class UnsetSettings : SettingsOrdinalResultSet
        {
            public UnsetSettings()
                : base([]) { }
        }
    }
}
