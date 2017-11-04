using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Equivalence
{
    public class SettingsEquivalerBuilder : SettingsResultSetBuilder
    {
        private bool isMultipleRows = true;
        private ColumnType valuesDefaultType = ColumnType.Numeric;
        private Tolerance defaultTolerance = null;

        public void Setup(bool isMultipleRows)
        {
            isBuild = false;
            this.isMultipleRows = isMultipleRows;
        }

        public void Setup(ColumnType valuesDefaultType, Tolerance defaultTolerance)
        {
            isBuild = false;
            this.valuesDefaultType = valuesDefaultType;
            this.defaultTolerance = defaultTolerance;
        }

        public void Setup(ColumnType valuesDefaultType, string toleranceString)
        {
            isBuild = false;
            var tolerance = ToleranceFactory.Instantiate(valuesDefaultType, toleranceString);
            Setup(valuesDefaultType, tolerance);
        }

        protected override void OnCheck()
        {
            if (isMultipleRows)
            {
                PerformInconsistencyChecks();
                PerformSetsAndColumnsCheck(
                    SettingsIndexResultSet.KeysChoice.AllExpectLast
                    , SettingsIndexResultSet.ValuesChoice.Last);
            }

            PerformToleranceChecks();
            PerformDuplicationChecks();
        }

        protected void PerformToleranceChecks()
        {
            if (Tolerance.IsNullOrNone(defaultTolerance))
                return;

            if (defaultTolerance is NumericTolerance && valuesDefaultType != ColumnType.Numeric)
                throw new InvalidOperationException($"You cannot define a default type for values as '{valuesDefaultType}' and setup a numeric default tolerance.");
            if (defaultTolerance is TextTolerance && valuesDefaultType != ColumnType.Text)
                throw new InvalidOperationException($"You cannot define a default type for values as '{valuesDefaultType}' and setup a text default tolerance.");
            if (defaultTolerance is DateTimeTolerance && valuesDefaultType != ColumnType.DateTime)
                throw new InvalidOperationException($"You cannot define a default type for values as '{valuesDefaultType}' and setup a dateTime default tolerance.");
        }


        protected override void OnBuild()
        {
            BuildSettings(ColumnType.Text, valuesDefaultType, defaultTolerance);
        }

        protected override void BuildSettings(ColumnType keysDefaultType, ColumnType valuesDefaultType, Tolerance defaultTolerance)
        {
            if (isMultipleRows)
                base.BuildSettings(keysDefaultType, valuesDefaultType, defaultTolerance);
            else
                settings = new SettingsSingleRowResultSet(valuesDefaultType, defaultTolerance, definitionColumns);
        }
    }
}
