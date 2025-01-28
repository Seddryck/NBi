using NBi.Core.Scalar.Casting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup;

class CellRetrieverByOrdinal : CellRetriever
{
    public CellRetrieverByOrdinal(IEnumerable<IColumnDefinition> settings)
        : base(settings)
    { }

    public override KeyCollection GetColumns(IResultRow row)
    {
        var keys = new List<object>();
        foreach (var setting in Settings)
        {
            var index = ((ColumnOrdinalIdentifier)setting.Identifier).Ordinal;
            try
            {
                var value = FormatValue(setting.Type, row[index]);
                keys.Add(value);
            }
            catch (FormatException)
            {
                throw new NBiException($"In the column with index '{index}', NBi can't convert the value '{row[index]}' to the type '{setting.Type}'. Key columns must match with their respective types and don't support null, generic or interval values.");
            }
            catch (InvalidCastException ex)
            {
                if (ex.Message.Contains("Object cannot be cast from DBNull to other types"))
                    throw new NBiException($"In the column with index '{index}', NBi can't convert the value 'DBNull' to the type '{setting.Type}'. Key columns must match with their respective types and don't support null, generic or interval values.");
                else
                    throw;
            }
        }
        return new KeyCollection([.. keys]);
    }
}
