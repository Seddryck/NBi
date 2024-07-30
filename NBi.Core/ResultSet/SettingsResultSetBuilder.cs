using NBi.Core.ResultSet.Analyzer;
using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public abstract class SettingsResultSetBuilder
    {
        protected bool isBuild = false;

        protected SettingsOrdinalResultSet.KeysChoice keysSet;
        protected IEnumerable<string> nameKeys = [];
        protected SettingsOrdinalResultSet.ValuesChoice valuesSet;
        protected IEnumerable<string> nameValues = [];
        protected IReadOnlyList<IColumnDefinition> definitionColumns = [];

        protected ISettingsResultSet? settings;

        public void Setup(IEnumerable<string> nameKeys, IEnumerable<string> nameValues)
        {
            isBuild = false;
            this.nameKeys = nameKeys ?? [];
            this.nameValues = nameValues ?? [];
        }

        public void Setup(SettingsOrdinalResultSet.KeysChoice keysSet, SettingsOrdinalResultSet.ValuesChoice valuesSet)
        {
            isBuild = false;
            this.keysSet = keysSet;
            this.valuesSet = valuesSet;
        }

        public void Setup(IReadOnlyList<IColumnDefinition> definitionColumns)
        {
            isBuild = false;
            this.definitionColumns = definitionColumns ?? [];
        }


        protected void PerformInconsistencyChecks()
        {
            definitionColumns ??= [];

            if ((keysSet != 0 || valuesSet != 0)
                && (nameKeys.Any() || nameValues.Any()))
                throw new InvalidOperationException("The definition of your settings is not valid. You cannot mix properties applicable for an engine based on columns' index and properties for an engine based on columns' name.");

            if (!nameKeys.Any()
                && nameValues.Any()
                && !definitionColumns.Any(c => c.Role == ColumnRole.Key))
                throw new InvalidOperationException("You cannot define an engine based on columns' name and specify no keys. Specify at least one column as a key.");

            
            if ((nameKeys.Any() || nameValues.Any())
                && definitionColumns.Any(c => c.Identifier is ColumnOrdinalIdentifier))
                throw new InvalidOperationException("You cannot define an engine based on columns' name and specify some column's definitions where you explicitely give a value to the 'index' attribute. Use attribute 'index' in place of 'name'.");

            if (!IsByName() && keysSet == SettingsOrdinalResultSet.KeysChoice.First
                && definitionColumns.Any(c => (c.Identifier as ColumnOrdinalIdentifier)!.Ordinal == 0 && c.Role!=ColumnRole.Key)  
                && !definitionColumns.Any(c => (c.Identifier as ColumnOrdinalIdentifier)!.Ordinal != 0 && c.Role == ColumnRole.Key))
                throw new InvalidOperationException("You cannot define a dataset without key. You've define a unique key, then overriden this key as a value and never set another key. Review your columns' definition.");
        }

        protected void PerformSetsAndColumnsCheck(SettingsOrdinalResultSet.KeysChoice defaultKeysSet, SettingsOrdinalResultSet.ValuesChoice defaultValuesSet)
        {
            if (((keysSet != defaultKeysSet && keysSet != SettingsOrdinalResultSet.KeysChoice.None) || valuesSet != defaultValuesSet && valuesSet != SettingsOrdinalResultSet.ValuesChoice.None)
                && definitionColumns.Any(c => c.Identifier is ColumnNameIdentifier))
                throw new InvalidOperationException("You cannot define an engine based on columns' index and specify some columns' definition where you explicitely give a value to the 'name' attribute. Use attribute 'index' in place of 'name'.");
        }


        protected void PerformDuplicationChecks()
        {
            var duplicatedColumnsDef = definitionColumns
                                        .GroupBy(c => c.Identifier.Label)
                                        .Select(group => new
                                        {
                                            Label = group.Key,
                                            Count = group.Count()
                                        }).Where(group => group.Count > 1).ToList();

            if (duplicatedColumnsDef.Count > 0)
            {
                throw new InvalidOperationException(
                    string.Format("You cannot define an engine where the same column is defined more than once. The column{0} having the label{1} '{2}' {3} defined several times."
                        , duplicatedColumnsDef.Count > 1 ? "s" : string.Empty
                        , duplicatedColumnsDef.Count > 1 ? "s" : string.Empty
                        , string.Join("', '", duplicatedColumnsDef.Select(x => x.Label).ToArray())
                        , duplicatedColumnsDef.Count > 1 ? "are" : "is"));
            }
        }

        public void Build()
        {
            OnCheck();
            OnBuild();
            isBuild = true;
        }

        protected abstract void OnCheck();

        protected abstract void OnBuild();
        
        protected virtual bool IsByName() =>
            nameKeys.Any()
            || nameValues.Any()
            || definitionColumns.Any(c => c.Identifier is ColumnNameIdentifier);

        protected virtual void BuildSettings(ColumnType keysDefaultType, ColumnType valuesDefaultType, Tolerance defaultTolerance)
        {
            if (IsByName())
                settings = new SettingsNameResultSet(valuesDefaultType, defaultTolerance, GetAllColumns(keysDefaultType, valuesDefaultType));
            else
                settings = new SettingsOrdinalResultSet(keysSet, valuesSet, valuesDefaultType, defaultTolerance, definitionColumns);
        }

        protected IEnumerable<IColumnDefinition> GetAllColumns(ColumnType keysDefaultType, ColumnType valuesDefaultType)
        {
            return nameKeys.Select(x => new Column(new ColumnNameIdentifier(x), ColumnRole.Key, keysDefaultType))
                                .Union(nameValues.Select(x => new Column(new ColumnNameIdentifier(x), ColumnRole.Value, valuesDefaultType))
                                .Union(definitionColumns)
                                );
        }

        public ISettingsResultSet GetSettings()
        {
            if (!isBuild || settings is null)
                throw new InvalidOperationException();

            return settings;
        }
    }
}
