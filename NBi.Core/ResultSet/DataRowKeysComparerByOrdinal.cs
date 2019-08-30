using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowKeysComparerByOrdinal : DataRowKeysComparer
    {
        private readonly SettingsOrdinalResultSet settings;

        public DataRowKeysComparerByOrdinal(SettingsOrdinalResultSet settings, int columnCount)
        {
            this.settings = settings;
            settings.ApplyTo(columnCount);
        }
        
        protected override bool CheckKeysExist(DataRow dr)
        {
            return settings.GetLastKeyColumnOrdinal() < dr.Table.Columns.Count;
        }
        
        public override KeyCollection GetKeys(DataRow row)
        {
            var keys = new List<object>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (settings.GetColumnRole(i) == ColumnRole.Key)
                {
                    try
                    {
                        var value = FormatValue(settings.GetColumnType(i), row[i]);
                        keys.Add(value);
                    }
                    catch (FormatException)
                    {
                        var txt = "In the column with index '{0}', NBi can't convert the value '{0}' to the type '{1}'. Key columns must match with their respective types and don't support null, generic or interval values.";
                        var msg = string.Format(txt, i, row[i], settings.GetColumnType(i));
                        throw new NBiException(msg);
                    }
                    catch (InvalidCastException ex)
                    {
                        if (ex.Message.Contains("Object cannot be cast from DBNull to other types"))
                        {
                            var txt = "In the column with index '{0}', NBi can't convert the value 'DBNull' to the type '{1}'. Key columns must match with their respective types and don't support null, generic or interval values.";
                            var msg = string.Format(txt, i, row[i], settings.GetColumnType(i));
                            throw new NBiException(msg);
                        }
                        else
                            throw ex;
                    }
                }
            }
            return new KeyCollection(keys.ToArray());
        }
        
    }
}
