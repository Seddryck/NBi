using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Comparison
{
    public class SettingsComparerBuilder : SettingsResultSetBuilder
    {
        private bool isMultipleRows;
        private ColumnType valuesDefaultType;
        private Tolerance defaultTolerance;

        private ComparerKind kind = ComparerKind.EqualTo;

        public void Setup(bool isMultipleRows, SettingsIndexResultSet.KeysChoice keysSet, string nameKeys, SettingsIndexResultSet.ValuesChoice valuesSet
            , string nameValues, ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyList<IColumnDefinition> definitionColumns, ComparerKind kind)
        {
            isBuild = false;

            this.keysSet = keysSet;
            this.nameKeys = nameKeys.Replace(" ", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct(); 
            this.valuesSet = valuesSet;
            this.nameValues = nameValues.Replace(" ", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            this.definitionColumns = definitionColumns;

            if (isMultipleRows)
                PerformInconsistencyChecks();
            PerformDuplicationChecks();
            
            this.isMultipleRows = isMultipleRows;
            this.valuesDefaultType = valuesDefaultType;
            this.defaultTolerance = defaultTolerance;
            this.kind = kind;

            isSetup = true;
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
