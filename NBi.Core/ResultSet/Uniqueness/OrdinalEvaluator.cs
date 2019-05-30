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

namespace NBi.Core.ResultSet.Uniqueness
{
    public class OrdinalEvaluator : Evaluator
    {
        private new SettingsOrdinalResultSet Settings
        {
            get { return base.Settings as SettingsOrdinalResultSet; }
        }
        
        public OrdinalEvaluator()
            : base()
        { }

        public OrdinalEvaluator(SettingsOrdinalResultSet settings)
            : base(settings)
        {
        }

        protected override void PreliminaryChecks(DataTable x)
        {
            var columnsCount = x.Columns.Count;
            if (Settings == null)
                BuildDefaultSettings(columnsCount);
            else
                Settings.ApplyTo(columnsCount);

            WriteSettingsToDataTableProperties(x, Settings);
            CheckSettingsAndDataTable(x, Settings);
            CheckSettingsAndFirstRow(x, Settings);
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
        {
            return new DataRowKeysComparerByOrdinal(Settings, x.Columns.Count);
        }


        protected void WriteSettingsToDataTableProperties(DataTable dt, SettingsOrdinalResultSet settings)
        {
            foreach (DataColumn column in dt.Columns)
            {
                WriteSettingsToDataTableProperties(
                    column
                    , settings.GetColumnRole(column.Ordinal)
                    , settings.GetColumnType(column.Ordinal)
                    , null
                    , null
                );
            }
        }

        protected void CheckSettingsAndDataTable(DataTable dt, SettingsOrdinalResultSet settings)
        {
            var max = settings.GetMaxColumnOrdinalDefined();
            if (dt.Columns.Count <= max)
            {
                var exception = string.Format("You've defined a column with an index of {0}, meaning that your result set would have at least {1} columns but your result set has only {2} columns."
                    , max
                    , max + 1
                    , dt.Columns.Count);

                if (dt.Columns.Count == max && settings.GetMinColumnOrdinalDefined() == 1)
                    exception += " You've no definition for a column with an index of 0. Are you sure you'vent started to index at 1 in place of 0?";

                throw new EquivalerException(exception);
            }
        }

        protected void CheckSettingsAndFirstRow(DataTable dt, SettingsOrdinalResultSet settings)
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
            base.Settings = new SettingsOrdinalResultSet(
                columnsCount,
                SettingsOrdinalResultSet.KeysChoice.All,
                SettingsOrdinalResultSet.ValuesChoice.None);
        }
    }
}
