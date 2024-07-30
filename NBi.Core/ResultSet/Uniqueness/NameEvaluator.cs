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
    public class NameEvaluator : Evaluator
    {
        private new SettingsNameResultSet Settings
        {
            get { return (SettingsNameResultSet)base.Settings; }
        }
        
        public NameEvaluator(SettingsNameResultSet settings)
            : base(settings)
        { }

        protected override void PreliminaryChecks(IResultSet x)
        {
            if (base.Settings == null)
                throw new InvalidOperationException();

            RemoveIgnoredColumns(x, Settings);

            WriteSettingsToDataTableProperties(x, Settings);
            CheckSettingsAndDataTable(x, Settings);
            CheckSettingsAndFirstRow(x, Settings);
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x)
        {
            return new DataRowKeysComparerByName(Settings);
        }

        protected void WriteSettingsToDataTableProperties(IResultSet dt, SettingsNameResultSet settings)
        {
            foreach (var column in dt.Columns)
            {
                column.SetProperties(
                    settings.GetColumnRole(column.Name)
                    , settings.GetColumnType(column.Name)
                );
            }
        }

        protected void CheckSettingsAndDataTable(IResultSet dt, SettingsNameResultSet settings)
        {
            var missingColumns = new List<KeyValuePair<string, string>>();
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
                var allColumnsHaveNoName = true;
                foreach (var column in dt.Columns)
                    allColumnsHaveNoName &= column.Name.StartsWith("No name");

                var exception = string.Format("You've defined {0} column{1} named '{2}' as key{1} or value{1} but there is no column with {3} name{1} in the resultset. {4}When using comparison by columns' name, you must ensure that all columns defined as keys and values are effectively available in the result-set."
                    , missingColumns.Count > 1 ? "some" : "a"
                    , missingColumns.Count > 1 ? "s" : string.Empty
                    , string.Join("', '", missingColumns.Select(kv => kv.Key))
                    , missingColumns.Count > 1 ? "these" : "this"
                    , allColumnsHaveNoName ? "None of the result-set's columns have a name. " : string.Empty
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
                var columnName = dt.GetColumn(i)?.Name ?? throw new ArgumentOutOfRangeException();
                CheckSettingsFirstRowCell(
                        settings.GetColumnRole(columnName)
                        , settings.GetColumnType(columnName)
                        , dt.GetColumn(columnName)!
                        , dr.IsNull(columnName) ? DBNull.Value : dr[columnName]!
                        ,
                            [
                                "The column named '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                                , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                                , "The column named '{0}' is expecting a date & time value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                            ]
                );
            }
        }

        protected void RemoveIgnoredColumns(IResultSet dt, SettingsNameResultSet settings)
        {
            var i = 0;

            while (i < dt.ColumnCount)
            {
                if (settings.GetColumnRole(dt.GetColumn(i)!.Name) == ColumnRole.Ignore)
                    dt.GetColumn(i)!.Remove();
                else
                    i++;
            }
        }
    }
}
