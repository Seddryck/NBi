using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup
{
    class KeysRetrieverByName : KeysRetriever
    {
        public KeysRetrieverByName(IEnumerable<IColumnDefinition> settings)
            : base(settings)
        { }

        public override KeyCollection GetKeys(DataRow row)
        {
            var keys = new List<object>();
            foreach (var setting in Settings)
            {
                try
                {
                    var value = FormatValue(setting.Type, row[setting.Name]);
                    keys.Add(value);
                }
                catch (FormatException)
                {
                    var txt = "In the column with name '{0}', NBi can't convert the value '{1}' to the type '{2}'. Key columns must match with their respective types and don't support null, generic or interval values.";
                    var msg = string.Format(txt, setting.Name, row[setting.Name], setting.Type);
                    throw new NBiException(msg);
                }
                catch (InvalidCastException ex)
                {
                    if (ex.Message.Contains("Object cannot be cast from DBNull to other types"))
                    {
                        var txt = "In the column with name '{0}', NBi can't convert the value 'DBNull' to the type '{1}'. Key columns must match with their respective types and don't support null, generic or interval values.";
                        var msg = string.Format(txt, setting.Name, row[setting.Name], setting.Type);
                        throw new NBiException(msg);
                    }
                    else
                        throw ex;
                }
            }
            return new KeyCollection(keys.ToArray());
        }
    }
}
