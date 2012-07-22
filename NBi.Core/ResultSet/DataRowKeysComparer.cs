using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class DataRowKeysComparer : IEqualityComparer<DataRow>
    {
        private IEnumerable<int> _keysPos;
        
        public DataRowKeysComparer()
        {
            _keysPos = new int[] { 0 };
        }
        
        public DataRowKeysComparer(IEnumerable<int> keysPos)
        {
            _keysPos = keysPos;
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
            return _keysPos.Max() < dr.Table.Columns.Count;
        }

        public int GetHashCode(DataRow obj)
        {
            var values = obj.ItemArray.Where<object>((o, i) => _keysPos.Contains(i));
            int hash = 0;
            foreach (var value in values)
            {
                hash = (hash * 397) ^ value.GetHashCode();
            }
            return hash;

        }
    }
}
