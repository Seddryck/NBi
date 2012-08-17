using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowKeysComparer : IEqualityComparer<DataRow>
    {
        private ResultSetComparisonSettings _settings;
               
        public DataRowKeysComparer(ResultSetComparisonSettings settings, int columnCount)
        {
            _settings = settings;
            settings.ApplyTo(columnCount);
        }
        
        public bool Equals(DataRow x, DataRow y)
        {
            if (!CheckKeysExist(x))
                throw new ArgumentException("First datarow have not the the required key fields");
            if (!CheckKeysExist(y))
                throw new ArgumentException("Second datarow have not the the required key fields");

            return GetHashCode(x) == GetHashCode(y);
        }

        protected bool CheckKeysExist(DataRow dr)
        {
            return _settings.GetLastKeyColumnIndex() < dr.Table.Columns.Count;
        }

        public int GetHashCode(DataRow obj)
        {
            var values = obj.ItemArray.Where<object>((o, i) => _settings.IsKey(i));
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
    }
}
