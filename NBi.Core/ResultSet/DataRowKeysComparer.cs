using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowKeysComparer : IEqualityComparer<DataRow>
    {
        private readonly ResultSetComparisonSettings settings;

        public DataRowKeysComparer(ResultSetComparisonSettings settings, int columnCount)
        {
            this.settings = settings;
            settings.ApplyTo(columnCount);
        }

        public bool Equals(DataRow x, DataRow y)
        {
            if (!CheckKeysExist(x))
                throw new ArgumentException("First datarow has not the required key fields");
            if (!CheckKeysExist(y))
                throw new ArgumentException("Second datarow has not the required key fields");

            return GetHashCode(x) == GetHashCode(y);
        }

        protected bool CheckKeysExist(DataRow dr)
        {
            return settings.GetLastKeyColumnIndex() < dr.Table.Columns.Count;
        }

        public int GetHashCode(DataRow obj)
        {
            var values = obj.ItemArray.Where<object>((o, i) => settings.GetColumnRole(i)==ColumnRole.Key);
            int hash = 0;
            foreach (var value in values)
            {
                string v = null;
                if (value is IConvertible)
                    v = ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
                else
                    v = value.ToString();

                //Console.WriteLine("{0} {1} {2} {3}", value.ToString(), value.GetType(), v.ToString(), v.GetHashCode());

                hash = (hash * 397) ^ v.GetHashCode();

            }
            return hash;
        }

        public KeyCollection GetKeys(DataRow row)
        {
            var keys = new List<object>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                
                if (settings.GetColumnRole(i) == ColumnRole.Key)
                {
                    try
                    {
                        var value = FormatValue(i, row[i]);
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

        internal object FormatValue(int columnIndex, object value)
        {
            object v = null;
            if (settings.GetColumnType(columnIndex) == ColumnType.Numeric)
                v = new NumericConverter().Convert(value);
            else if (settings.GetColumnType(columnIndex) == ColumnType.DateTime)
                v = new DateTimeConverter().Convert(value);
            else if (settings.GetColumnType(columnIndex) == ColumnType.Boolean)
                v = new ThreeStateBooleanConverter().Convert(value);
            else
            {
                if (value is IConvertible)
                    v = ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
                else
                    v = value.ToString();
            }
            return v;
        }
    }
}
