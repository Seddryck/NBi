using NBi.Core.Scalar.Casting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup;

class CellRetrieverByName : CellRetriever
{
    public CellRetrieverByName(IEnumerable<IColumnDefinition> settings)
        : base(settings)
    { }

    public override KeyCollection GetColumns(IResultRow row)
    {
        var keys = new List<object>();
        foreach (var setting in Settings)
        {
            var name = ((ColumnNameIdentifier)setting.Identifier).Name;
            try
            {
                var value = FormatValue(setting.Type, row[name]);
                keys.Add(value);
            }
            catch (ArgumentException ex)
            {
                var columnNames = row.Parent.Columns.Select(x => x.Name);
                throw new NBiException($"{ex.Message} This table contains the following column{(columnNames.Count()>1 ? "s" : string.Empty)}: '{string.Join("', '",  columnNames)}'.");
            }
            catch (FormatException)
            {
                throw new NBiException($"In the column with name '{name}', NBi can't convert the value '{row[name]}' to the type '{setting.Type}'. Key columns must match with their respective types and don't support null, generic or interval values.");
            }
            catch (InvalidCastException ex)
            {
                if (ex.Message.Contains("Object cannot be cast from DBNull to other types"))
                {
                    throw new NBiException($"In the column with name '{name}', NBi can't convert the value 'DBNull' to the type '{setting.Type}'. Key columns must match with their respective types and don't support null, generic or interval values.");
                }
                else
                    throw;
            }
        }
        return new KeyCollection([.. keys]);
    }
}
