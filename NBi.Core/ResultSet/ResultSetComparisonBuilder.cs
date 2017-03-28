using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class ResultSetComparisonBuilder
    {
        private bool isSetup = false;
        private bool isBuild = false;

        private bool isMultipleRows;
        private SettingsResultSetComparisonByIndex.KeysChoice keysDef;
        private string keyNames;
        private SettingsResultSetComparisonByIndex.ValuesChoice valuesDef;
        private string valueNames;
        private ColumnType valuesDefaultType;
        private NumericTolerance defaultTolerance;
        private IReadOnlyList<IColumnDefinition> columnsDef;

        private ISettingsResultSetComparison settings;
        private IResultSetComparer comparer;

        public void Setup(bool isMultipleRows, SettingsResultSetComparisonByIndex.KeysChoice keysDef, string keyNames, SettingsResultSetComparisonByIndex.ValuesChoice valuesDef, string valueNames, ColumnType valuesDefaultType, NumericTolerance defaultTolerance, IReadOnlyList<IColumnDefinition> columnsDef)
        {
            isBuild = false;

            if (columnsDef == null)
                columnsDef = new List<IColumnDefinition>();

            if (isMultipleRows
                && (keysDef != 0 || valuesDef != 0)
                && (!string.IsNullOrEmpty(keyNames) || !string.IsNullOrEmpty(valueNames)))
                throw new InvalidOperationException("The definition of your comparison is not valid. You cannot mix values applicable for a comparison by columns' index and by columns' name.");

            if (isMultipleRows
                && !string.IsNullOrEmpty(valueNames)
                && string.IsNullOrEmpty(keyNames) && !columnsDef.Any(c => c.Role == ColumnRole.Key))
                throw new InvalidOperationException("You cannot define a comparison by name and specify no keys. Specify at least one column as a key.");

            if (isMultipleRows
                && (keysDef != 0 || valuesDef != 0)
                && columnsDef.Any(c => !string.IsNullOrEmpty(c.Name)))
                throw new InvalidOperationException("You cannot define a comparison by index and specify some column's definitions where you explicitely give a value to the 'name' attribute. Use attribute 'index' in place of 'name'.");

            if (isMultipleRows
                && (!string.IsNullOrEmpty(keyNames) || !string.IsNullOrEmpty(valueNames))
                && columnsDef.Any(c => c.Index != 0))
                throw new InvalidOperationException("You cannot define a comparison by name and specify some column's definitions where you explicitely give a value to the 'index' attribute. Use attribute 'index' in place of 'name'.");

            if (isMultipleRows
                && columnsDef.Any(c => c.Type == ColumnType.Table && c.Role == ColumnRole.Key))
                throw new InvalidOperationException("You cannot specify a column's as a sub-table and as a key.");

            if (columnsDef.Any(c => c.Index != 0 && !string.IsNullOrEmpty(c.Name)))
                throw new InvalidOperationException("You cannot define some column's definitions where you explicitely give a value to the 'index' attribute and to the 'name' attribute. Use attribute 'index' or 'name' but not both.");

            var duplicatedColumnsDefByIndex = columnsDef
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

            var duplicatedColumnsDefByName = columnsDef
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


            this.isMultipleRows = isMultipleRows;
            this.keysDef = keysDef;
            this.keyNames = keyNames;
            this.valuesDef = valuesDef;
            this.valueNames = valueNames;
            this.valuesDefaultType = valuesDefaultType;
            this.defaultTolerance = defaultTolerance;
            this.columnsDef = columnsDef;

            isSetup = true;

        }

        public void Setup(SettingsResultSetComparisonByName settings)
        {
            isBuild = false;
            this.settings = settings;
            isSetup = true;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (settings == null)
                BuildSettings();

            BuildComparer();
            isBuild = true;
        }

        protected void BuildComparer()
        {
            if (settings is SettingsSingleRowComparison)
                comparer = new SingleRowComparer(settings as SettingsSingleRowComparison);

            else if (settings is SettingsResultSetComparisonByIndex)
                comparer = new ResultSetComparerByIndex(settings as SettingsResultSetComparisonByIndex);

            else if (settings is SettingsResultSetComparisonByName)
                comparer = new ResultSetComparerByName(settings as SettingsResultSetComparisonByName);
        }

        protected void BuildSettings()
        {


            var isByName = !string.IsNullOrEmpty(keyNames)
                        || !string.IsNullOrEmpty(valueNames)
                        || columnsDef.Any(c => !string.IsNullOrEmpty(c.Name));

            if (isMultipleRows && isByName)
            {
                var keyNamesList = new List<string>();
                if (!string.IsNullOrEmpty(keyNames))
                {
                    var keys = keyNames.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    keyNamesList.AddRange((keys.Select(x => x.Trim()).ToList()));
                }

                var valueNamesList = new List<string>();
                if (!string.IsNullOrEmpty(valueNames))
                {
                    var values = valueNames.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    valueNamesList.AddRange((values.Select(x => x.Trim()).ToList()));
                }

                var allColumns =
                    keyNamesList.Select(x => new Column() { Name = x, Role = ColumnRole.Key, Type = ColumnType.Text })
                    .Union(valueNamesList.Select(x => new Column() { Name = x, Role = ColumnRole.Value, Type = valuesDefaultType })
                    .Union(columnsDef)
                    );

                var subGroups = allColumns.Select(x => x.Name.Split(new char[] { '.' }, 2)).Where(x => x.Length == 2).GroupBy(x => x[0]).Select((x, y) => x.Key);
                var subSettings = new List<SettingsResultSetComparisonByName>();
                foreach (var subGroup in subGroups)
                {
                    subSettings.Add(new SettingsResultSetComparisonByName(subGroup, allColumns.Where(x => x.Name.StartsWith(subGroup + ".")), valuesDefaultType, defaultTolerance));
                }


                settings = new SettingsResultSetComparisonByName(allColumns, valuesDefaultType, defaultTolerance, subSettings);
            }

            else if (isMultipleRows && !isByName)
            {
                settings = new SettingsResultSetComparisonByIndex(keysDef, valuesDef, valuesDefaultType, defaultTolerance, columnsDef);
            }

            else if (!isMultipleRows)
            {
                settings = new SettingsSingleRowComparison(valuesDefaultType, defaultTolerance, columnsDef);
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

        public IResultSetComparer GetComparer()
        {
            if (!isBuild)
                throw new InvalidOperationException();

            return comparer;
        }

    }
}
