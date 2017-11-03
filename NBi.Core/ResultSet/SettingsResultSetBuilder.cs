using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Comparer;
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
        protected bool isSetup = false;
        protected bool isBuild = false;

        protected SettingsIndexResultSet.KeysChoice keysSet;
        protected IEnumerable<string> nameKeys;
        protected SettingsIndexResultSet.ValuesChoice valuesSet;
        protected IEnumerable<string> nameValues;
        protected IReadOnlyList<IColumnDefinition> definitionColumns;

        protected ISettingsResultSet settings;

        public void Setup(SettingsIndexResultSet.KeysChoice keysSet, SettingsIndexResultSet.ValuesChoice valuesSet, IReadOnlyList<IColumnDefinition> definitionColumns)
        {
            isBuild = false;

            this.keysSet = keysSet;
            nameKeys = (new List<string>()).AsReadOnly();
            this.valuesSet = valuesSet;
            nameValues = (new List<string>()).AsReadOnly();
            this.definitionColumns = definitionColumns;

            PerformInconsistencyChecks();
            PerformDuplicationChecks();

            isSetup = true;
        }

        protected void PerformInconsistencyChecks()
        {
            if (definitionColumns == null)
                definitionColumns = new List<IColumnDefinition>();

            if ((keysSet != 0 || valuesSet != 0)
                && (nameKeys.Count() > 0 || nameValues.Count() > 0))
                throw new InvalidOperationException("The definition of your settings is not valid. You cannot mix properties applicable for an engine based on columns' index and properties for an engine based on columns' name.");

            if (nameValues.Count() == 0
                && nameKeys.Count() > 0
                && !definitionColumns.Any(c => c.Role == ColumnRole.Key))
                throw new InvalidOperationException("You cannot define an engine based on columns' name and specify no keys. Specify at least one column as a key.");

            if ((keysSet != 0 || valuesSet != 0)
                && definitionColumns.Any(c => !string.IsNullOrEmpty(c.Name)))
                throw new InvalidOperationException("You cannot define an engine based on columns' index and specify some columns' definition where you explicitely give a value to the 'name' attribute. Use attribute 'index' in place of 'name'.");

            if ((nameKeys.Count() > 0 || nameValues.Count() > 0)
                && definitionColumns.Any(c => c.Index != 0))
                throw new InvalidOperationException("You cannot define an engine based on columns' name and specify some column's definitions where you explicitely give a value to the 'index' attribute. Use attribute 'index' in place of 'name'.");

            if (definitionColumns.Any(c => c.Index != 0 && !string.IsNullOrEmpty(c.Name)))
                throw new InvalidOperationException("You cannot define some columns' definitions where you explicitely give a value to the 'index' attribute and to the 'name' attribute. Use attribute 'index' or 'name' but not both.");
        }

        protected void PerformDuplicationChecks()
        {
            var duplicatedColumnsDefByIndex = definitionColumns
                                        .Where(c => c.Index != 0)
                                        .GroupBy(c => c.Index)
                                        .Select(group => new
                                        {
                                            Index = group.Key,
                                            Count = group.Count()
                                        }).Where(group => group.Count > 1).ToList();

            if (duplicatedColumnsDefByIndex.Count > 0)
            {
                throw new InvalidOperationException(
                    string.Format("You cannot define an engine where the same column is defined more than once. The column{0} having the index{1} '{2}' {3} defined several times."
                        , duplicatedColumnsDefByIndex.Count > 1 ? "s" : string.Empty
                        , duplicatedColumnsDefByIndex.Count > 1 ? "es" : string.Empty
                        , string.Join("', '", duplicatedColumnsDefByIndex.Select(x => x.Index.ToString()).ToArray())
                        , duplicatedColumnsDefByIndex.Count > 1 ? "are" : "is"));
            }

            var duplicatedColumnsDefByName = definitionColumns
                                        .Where(c => !string.IsNullOrEmpty(c.Name))
                                        .GroupBy(c => c.Name)
                                        .Select(group => new
                                        {
                                            Name = group.Key,
                                            Count = group.Count()
                                        }).Where(group => group.Count > 1).ToList();

            if (duplicatedColumnsDefByName.Count > 0)
            {
                throw new InvalidOperationException(
                    string.Format("You cannot define a comparison where the same column is defined more than once. The column{0} having the name{1} '{2}' {3} defined several times."
                        , duplicatedColumnsDefByName.Count > 1 ? "s" : string.Empty
                        , duplicatedColumnsDefByName.Count > 1 ? "s" : string.Empty
                        , string.Join("', '", duplicatedColumnsDefByName.Select(x => x.Name).ToArray())
                        , duplicatedColumnsDefByName.Count > 1 ? "are" : "is"));
            }
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            OnBuild();

            isBuild = true;
        }

        protected abstract void OnBuild();
        
        protected virtual bool IsByName() => 
            nameKeys.Count()>0
            || nameValues.Count() > 0
            || definitionColumns.Any(c => !string.IsNullOrEmpty(c.Name));

        protected virtual void BuildSettings(ColumnType keysDefaultType, ColumnType valuesDefaultType, Tolerance defaultTolerance)
        {
            if (IsByName())
            {
                var allColumns =
                    nameKeys.Select(x => new Column() { Name = x, Role = ColumnRole.Key, Type = keysDefaultType })
                    .Union(nameValues.Select(x => new Column() { Name = x, Role = ColumnRole.Value, Type = valuesDefaultType })
                    .Union(definitionColumns)
                    );

                settings = new SettingsNameResultSet(valuesDefaultType, defaultTolerance, allColumns);
            }

            else
            {
                settings = new SettingsIndexResultSet(keysSet, valuesSet, valuesDefaultType, defaultTolerance, definitionColumns);
            }
        }

        public ISettingsResultSet GetSettings()
        {
            if (!isBuild)
                throw new InvalidOperationException();

            return settings;
        }
    }
}
