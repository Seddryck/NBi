using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public class ResultSetComparaisonSettings
    {
        public IList<int> KeyColumnIndexes { get; private set; }
        public IList<int> ValueColumnIndexes {  get; private set; }
        protected IList<decimal> _tolerances;
        public decimal Tolerances(int index)
        {
            for (int i = 0; i < ValueColumnIndexes.Count; i++)
            {
                if (ValueColumnIndexes[i] == index)
                    return _tolerances[i];
            }
            throw new ArgumentException();
        }

        public ResultSetComparaisonSettings() : this (new List<int>() {0}, new List<int>() {1}, 0)
        {
        }

        public ResultSetComparaisonSettings(decimal tolerance)
            : this(new List<int>() { 0 }, new List<int>() { 1 }, tolerance)
        {

        }

        protected ResultSetComparaisonSettings(IList<int> keyColumnIndexes, IList<int> valueColumnIndexes)
        {
            KeyColumnIndexes = keyColumnIndexes;
            ValueColumnIndexes = valueColumnIndexes;
        }

        public ResultSetComparaisonSettings(IList<int> keyColumnIndexes, IList<int> valueColumnIndexes, decimal tolerance)
            : this(keyColumnIndexes, valueColumnIndexes) 
        {
            _tolerances = new List<decimal>(valueColumnIndexes.Count);
            for (int i = 0; i < valueColumnIndexes.Count; i++)
                _tolerances.Add(tolerance);
        }

        public ResultSetComparaisonSettings(IList<int> keyColumnIndexes, IList<int> valueColumnIndexes, IList<decimal> tolerances)
            : this(keyColumnIndexes, valueColumnIndexes) 
        {
            if (valueColumnIndexes.Count != tolerances.Count)
                throw new ArgumentException();
            _tolerances = tolerances;
        }

        public ResultSetComparaisonSettings(int keyColumnCount, int valueColumnCount, decimal tolerance)
        {
            KeyColumnIndexes = new List<int>(keyColumnCount);
            for (int i = 0; i < keyColumnCount; i++)
                KeyColumnIndexes.Add(i);

            ValueColumnIndexes = new List<int>(valueColumnCount);
            _tolerances = new List<decimal>(valueColumnCount);
            for (int i = 0; i < valueColumnCount; i++)
            {
                ValueColumnIndexes.Add(i + keyColumnCount);
                _tolerances.Add(tolerance);
            }

        }
    }
}
