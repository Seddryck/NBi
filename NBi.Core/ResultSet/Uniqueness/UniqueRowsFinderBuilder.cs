using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class UniqueRowsFinderBuilder
    {
        private bool isSetup = false;
        private bool isBuild = false;

        private SettingsResultSetComparisonByIndex.KeysChoice keysDef;
        private SettingsResultSetComparisonByIndex.ValuesChoice valuesDef;

        private ColumnType valuesDefaultType;
        private IReadOnlyList<IColumnDefinition> definitionColumns;

        private ISettingsResultSetComparison settings;

        public void Setup(SettingsResultSetComparisonByIndex.KeysChoice keysDef, SettingsResultSetComparisonByIndex.ValuesChoice valuesDef, IReadOnlyList<IColumnDefinition> columnsDefs)
        {
            isBuild = false;

            if (definitionColumns == null)
                definitionColumns = new List<IColumnDefinition>();

            if ((keysDef != 0 || valuesDef != 0)
                && this.definitionColumns.Any(c => !string.IsNullOrEmpty(c.Name)))
                throw new InvalidOperationException("You cannot define a comparison by index and specify some column's definitions where you explicitely give a value to the 'name' attribute. Use attribute 'index' in place of 'name'.");

            if (definitionColumns.Any(c => c.Index != 0 && !string.IsNullOrEmpty(c.Name)))
                throw new InvalidOperationException("You cannot define some column's definitions where you explicitely give a value to the 'index' attribute and to the 'name' attribute. Use attribute 'index' or 'name' but not both.");

            var duplicatedColumnsDefByIndex = columnsDefs
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
                    string.Format("You cannot define a comparison where the same column is defined more than once. The column{0} having the index{1} '{2}' {3} defined several times."
                        , duplicatedColumnsDefByIndex.Count > 1 ? "s" : string.Empty
                        , duplicatedColumnsDefByIndex.Count > 1 ? "es" : string.Empty
                        , string.Join("', '", duplicatedColumnsDefByIndex.Select(x => x.Index.ToString()).ToArray())
                        , duplicatedColumnsDefByIndex.Count > 1 ? "are" : "is"));
            }

            var duplicatedColumnsDefByName = columnsDefs
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


            this.keysDef = keysDef;
            this.valuesDef = valuesDef;
            this.definitionColumns = columnsDefs;

            isSetup = true;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (settings == null)
                BuildSettings();

            isBuild = true;
        }

        protected void BuildSettings()
        {
            var isByName = definitionColumns.Any(c => !string.IsNullOrEmpty(c.Name));

            if (isByName)
            {
                var keyNamesList = new List<string>();
                var valueNamesList = new List<string>();
                
                var allColumns =
                    keyNamesList.Select(x => new Column() { Name = x, Role = ColumnRole.Key, Type = ColumnType.Text })
                    .Union(valueNamesList.Select(x => new Column() { Name = x, Role = ColumnRole.Value, Type = valuesDefaultType })
                    .Union(definitionColumns)
                    );

                settings = new SettingsResultSetComparisonByName(valuesDefaultType, null, allColumns);
            }

            else if (!isByName)
            {
                settings = new SettingsResultSetComparisonByIndex(keysDef, valuesDef, valuesDefaultType, null, definitionColumns);
            }
            
            else
                throw new InvalidOperationException();

        }

        public ISettingsResultSetComparison GetSettings()
        {
            if (!isBuild)
                throw new InvalidOperationException();

            return settings;
        }
    }
}
