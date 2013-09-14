using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet
{
	public class ResultSetComparisonSettings
	{
		public enum KeysChoice
		{
			[XmlEnum(Name = "first")]
			First = 0,
			[XmlEnum(Name = "all-except-last")]
			AllExpectLast = 1,
			[XmlEnum(Name = "all")]
			All = 2
		}

		public enum ValuesChoice
		{
			[XmlEnum(Name = "all-except-first")]
			AllExpectFirst = 0,
			[XmlEnum(Name = "last")]
			Last = 1
		}

		public KeysChoice KeysDef { get; set; }
		private ValuesChoice ValuesDef { get; set; }
		private ICollection<IColumnDefinition> ColumnsDef { get; set; }
		private string DefaultTolerance { get; set; }

		public bool IsKey(int index)
		{
		   
			if (ColumnsDef.Any( c => c.Index==index && c.Role!=ColumnRole.Key))
				return false;
			
			if (ColumnsDef.Any( c => c.Index==index && c.Role==ColumnRole.Key))
				return true;

			switch (KeysDef)
			{
				case KeysChoice.First:
					return index==0;
				case KeysChoice.AllExpectLast:
					return index!=GetLastColumnIndex();
				case KeysChoice.All:
					return true;
			}

			return false;
		}

		public bool IsValue(int index)
		{
			if (ColumnsDef.Any(c => c.Index == index && c.Role != ColumnRole.Value))
				return false;

			if (ColumnsDef.Any(c => c.Index == index && c.Role == ColumnRole.Value))
				return true;

			switch (KeysDef)
			{
				case KeysChoice.First:
					if (index == 0) return false; 
					break;
				case KeysChoice.AllExpectLast:
					if (index != GetLastColumnIndex()) return false;
					break;
				case KeysChoice.All:
					return false;
			}

			switch (ValuesDef)
			{
				case ValuesChoice.AllExpectFirst:
					return index != 0;
				case ValuesChoice.Last:
					return index == GetLastColumnIndex();
			}

			return false;
		}

		public ColumnRole GetColumnRole(int index)
		{
			if (IsKey(index))
				return ColumnRole.Key;
			else if (IsValue(index))
				return ColumnRole.Value;
			else
				return ColumnRole.Ignore;
		}

		public ColumnType GetColumnType(int index)
		{
			if (IsNumeric(index))
				return ColumnType.Numeric;
			if (IsDateTime(index))
				return ColumnType.DateTime;
			if (IsBoolean(index))
				return ColumnType.Boolean;
			else
				return ColumnType.Text;
		}

		public bool IsNumeric(int index)
		{
			if (ColumnsDef.Any(c => c.Index == index && c.Type != ColumnType.Numeric))
				return false;

			if (ColumnsDef.Any(c => c.Index == index && c.Type == ColumnType.Numeric))
				return true;

			return IsValue(index);
		}

		public bool IsDateTime(int index)
		{
			if (ColumnsDef.Any(c => c.Index == index && c.Type != ColumnType.DateTime))
				return false;

			if (ColumnsDef.Any(c => c.Index == index && c.Type == ColumnType.DateTime))
				return true;

			return false;
		}

		public bool IsBoolean(int index)
		{
			if (ColumnsDef.Any(c => c.Index == index && c.Type != ColumnType.Boolean))
				return false;

			if (ColumnsDef.Any(c => c.Index == index && c.Type == ColumnType.Boolean))
				return true;

			return false;
		}

		public string GetTolerance(int index)
		{
			if (!IsNumeric(index) && !IsDateTime(index))
				return string.Empty;
			
			var col = ColumnsDef.FirstOrDefault(c => c.Index == index);
			if (col == null)
				return DefaultTolerance;

			if (col.IsToleranceSpecified)
				return col.Tolerance;
			else
				return DefaultTolerance;
		}

		public int GetLastColumnIndex()
		{
			if (!isLastColumnIndexDefined)
				throw new InvalidOperationException("You must call the method ApplyTo() before trying to call GetLastColumnIndex()");
			
			return lastColumnIndex;
		}

		public int GetMinColumnIndexDefined()
		{
			if (ColumnsDef.Count > 0)
				return ColumnsDef.Min(cd => cd.Index);
			else
				return -1;
		}

		public int GetMaxColumnIndexDefined()
		{
			if (ColumnsDef.Count > 0)
				return ColumnsDef.Max(cd => cd.Index);
			else
				return -1;
		}

		public int GetLastKeyColumnIndex()
		{
			var max = 0;
			for (int i = 0; i < GetLastColumnIndex(); i++)
			{
				if (IsKey(i))
					max = i;
			}

			return max;
		}

		private bool isLastColumnIndexDefined = false;
		private int lastColumnIndex;

		public void ApplyTo(int columnCount)
		{
			isLastColumnIndexDefined = true;
			lastColumnIndex = columnCount-1;
		}
		
		//public IList<int> KeyColumnIndexes { get; private set; }
		//public IList<int> ValueColumnIndexes {  get; private set; }
		//protected IList<decimal> _tolerances;
		//public decimal Tolerances(int index)
		//{
		//    for (int i = 0; i < ValueColumnIndexes.Count; i++)
		//    {
		//        if (ValueColumnIndexes[i] == index)
		//            return _tolerances[i];
		//    }
		//    throw new ArgumentException();
		//}

		public ResultSetComparisonSettings(int columnsCount, KeysChoice keysDef, ValuesChoice valuesDef)
			: this(keysDef, valuesDef, "0", null)
		{
			ApplyTo(columnsCount);
		}

		public ResultSetComparisonSettings(KeysChoice keysDef, ValuesChoice valuesDef, ICollection<IColumnDefinition> columnsDef)
            : this(keysDef, valuesDef, "0", columnsDef)
		{
		}

		public ResultSetComparisonSettings(KeysChoice keysDef, ValuesChoice valuesDef, string defaultTolerance)
			: this(keysDef, valuesDef, defaultTolerance, null)
		{
		}

		public ResultSetComparisonSettings(KeysChoice keysDef, ValuesChoice valuesDef, string defaultTolerance, ICollection<IColumnDefinition> columnsDef)
		{
			KeysDef = keysDef;
			ValuesDef = valuesDef;
			DefaultTolerance = defaultTolerance;
			if (columnsDef != null)
				ColumnsDef = columnsDef;
			else
				ColumnsDef = new List<IColumnDefinition>(0);
		}

		//public ResultSetComparaisonSettings() : this (new List<int>() {0}, new List<int>() {1}, 0)
		//{
		//}

		//public ResultSetComparaisonSettings(decimal tolerance)
		//    : this(new List<int>() { 0 }, new List<int>() { 1 }, tolerance)
		//{

		//}

		//protected ResultSetComparaisonSettings(IList<int> keyColumnIndexes, IList<int> valueColumnIndexes)
		//{
		//    KeyColumnIndexes = keyColumnIndexes;
		//    ValueColumnIndexes = valueColumnIndexes;
		//}

		//public ResultSetComparaisonSettings(IList<int> keyColumnIndexes, IList<int> valueColumnIndexes, decimal tolerance)
		//    : this(keyColumnIndexes, valueColumnIndexes) 
		//{
		//    _tolerances = new List<decimal>(valueColumnIndexes.Count);
		//    for (int i = 0; i < valueColumnIndexes.Count; i++)
		//        _tolerances.Add(tolerance);
		//}

		//public ResultSetComparaisonSettings(IList<int> keyColumnIndexes, IList<int> valueColumnIndexes, IList<decimal> tolerances)
		//    : this(keyColumnIndexes, valueColumnIndexes) 
		//{
		//    if (valueColumnIndexes.Count != tolerances.Count)
		//        throw new ArgumentException();
		//    _tolerances = tolerances;
		//}

		//public ResultSetComparaisonSettings(int keyColumnCount, int valueColumnCount, decimal tolerance)
		//{
		//    KeyColumnIndexes = new List<int>(keyColumnCount);
		//    for (int i = 0; i < keyColumnCount; i++)
		//        KeyColumnIndexes.Add(i);

		//    ValueColumnIndexes = new List<int>(valueColumnCount);
		//    _tolerances = new List<decimal>(valueColumnCount);
		//    for (int i = 0; i < valueColumnCount; i++)
		//    {
		//        ValueColumnIndexes.Add(i + keyColumnCount);
		//        _tolerances.Add(tolerance);
		//    }

		//}

		public void ConsoleDisplay()
		{
			//Console.Write("Indexes: |");
			//foreach (var kci in KeyColumnIndexes)
			//    Console.Write("{0} | ", kci);
			//Console.WriteLine();

			//Console.Write("Values: |");
			//for (int i = 0; i < ValueColumnIndexes.Count; i++)
			//    Console.Write("{0} (+/- {1}) |", ValueColumnIndexes[i], "?");
			//Console.WriteLine();
		}
	}
}
