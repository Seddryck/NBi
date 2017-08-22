using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
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

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            var isByName = !string.IsNullOrEmpty(keyNames)
                        || !string.IsNullOrEmpty(valueNames)
                        || columnsDef.Any(c => !string.IsNullOrEmpty(c.Name));

            if (isMultipleRows && isByName)
            {
                var settings = new SettingsResultSetComparisonByName(keyNames, valueNames, valuesDefaultType, defaultTolerance, columnsDef);
                comparer = new ResultSetComparerByName(settings);
                this.settings = settings;
            }
                
            else if (isMultipleRows && !isByName)
            {
                var settings = new SettingsResultSetComparisonByIndex(keysDef, valuesDef, valuesDefaultType, defaultTolerance, columnsDef);
                comparer = new ResultSetComparerByIndex(settings);
                this.settings = settings;
            }
                
            else if (!isMultipleRows)
            {
                var settings = new SettingsSingleRowComparison(valuesDefaultType, defaultTolerance, columnsDef);
                comparer = new SingleRowComparer(settings);
                this.settings = settings;
            }
            else
                throw new InvalidOperationException();

            isBuild = true;
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
