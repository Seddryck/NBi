using NBi.Core.Scalar.Casting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowKeysComparerByName : DataRowKeysComparer
    {
        private readonly SettingsNameResultSet settings;

        public DataRowKeysComparerByName(SettingsNameResultSet settings)
        {
            this.settings = settings;
        }

        protected override bool CheckKeysExist(IResultRow dr)
        {
            foreach (var columnName in settings.GetKeyNames())
            {
                if (!dr.Parent.ContainsColumn(columnName))
                    return false;
            }
            return true;
        }
        
        public override KeyCollection GetKeys(IResultRow row)
        {
            var keys = new List<object>();
            foreach (var keyName in settings.GetKeyNames())
            {
                try
                {
                    var value = FormatValue(settings.GetColumnType(keyName), row[keyName] ?? throw new NullReferenceException());
                    keys.Add(value);
                }
                catch (FormatException)
                {
                    var txt = "In the column with name '{0}', NBi can't convert the value '{1}' to the type '{2}'. Key columns must match with their respective types and don't support null, generic or interval values.";
                    var msg = string.Format(txt, keyName, row[keyName], settings.GetColumnType(keyName));
                    throw new NBiException(msg);
                }
                catch (InvalidCastException ex)
                {
                    if (ex.Message.Contains("Object cannot be cast from DBNull to other types"))
                    {
                        var txt = "In the column with name '{0}', NBi can't convert the value 'DBNull' to the type '{1}'. Key columns must match with their respective types and don't support null, generic or interval values.";
                        var msg = string.Format(txt, keyName, row[keyName], settings.GetColumnType(keyName));
                        throw new NBiException(msg);
                    }
                    else
                        throw;
                }
            }
            return new KeyCollection([.. keys]);
        }
    }
}
