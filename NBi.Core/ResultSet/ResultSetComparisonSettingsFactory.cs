using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.ResultSetComparisonByIndexSettings;

namespace NBi.Core.ResultSet
{
    public class ResultSetComparisonSettingsFactory
    {
        public IResultSetComparisonSettings Build(bool isMultipleRows, KeysChoice keysDef, string keyNames, ValuesChoice valuesDef, string valueNames, ColumnType valuesDefaultType, NumericTolerance defaultTolerance, IReadOnlyList<IColumnDefinition> columnsDef)
        {
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


            var isByName = !string.IsNullOrEmpty(keyNames)
                        || !string.IsNullOrEmpty(valueNames)
                        || columnsDef.Any(c => !string.IsNullOrEmpty(c.Name));

            if (isMultipleRows && isByName)
                return new ResultSetComparisonByNameSettings(keyNames, valueNames, valuesDefaultType, defaultTolerance, columnsDef);
            else if (isMultipleRows && !isByName)
                return new ResultSetComparisonByIndexSettings(keysDef, valuesDef, valuesDefaultType, defaultTolerance, columnsDef);
            else if (!isMultipleRows)
                return new ResultSetComparisonByIndexSettings(keysDef, valuesDef, valuesDefaultType, defaultTolerance, columnsDef);
            else
                throw new InvalidOperationException();
        }

    }
}
