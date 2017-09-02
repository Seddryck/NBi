using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet
{
	public class SettingsResultSetComparisonByIndex : SettingsResultSetComparison<int>
	{
    
		public enum KeysChoice
		{
			[XmlEnum(Name = "first")]
			First = 0,
			[XmlEnum(Name = "all-except-last")]
			AllExpectLast = 1,
            [XmlEnum(Name = "all")]
            All = 2,
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
        
		protected override bool IsKey(int index)
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
        
        protected override bool IsValue(int index)
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
        
        public override bool IsRounding(int index)
		{
			return ColumnsDef.Any(
					c => c.Index == index
					&& c.Role == ColumnRole.Value
					&& c.RoundingStyle != Comparer.Rounding.RoundingStyle.None
					&& !string.IsNullOrEmpty(c.RoundingStep));
		}
        
        public override Rounding GetRounding(int index)
		{
			if (!IsRounding(index))
				return null;

			return RoundingFactory.Build(ColumnsDef.Single(
					c => c.Index == index
					&& c.Role == ColumnRole.Value));
		}
        
        public override ColumnRole GetColumnRole(int index)
		{
            if (!cacheRole.ContainsKey(index))
            {
                if (IsKey(index))
                    cacheRole.Add(index,ColumnRole.Key);
                else if (IsValue(index))
                    cacheRole.Add(index,ColumnRole.Value);
                else
                    cacheRole.Add(index,ColumnRole.Ignore);
            }
            
            return cacheRole[index];
		}
        
        public override ColumnType GetColumnType(int index)
		{
            if (!cacheType.ContainsKey(index))
            {
                if (IsNumeric(index))
                    cacheType.Add(index, ColumnType.Numeric);
                else if (IsDateTime(index))
                    cacheType.Add(index, ColumnType.DateTime);
                else if (IsBoolean(index))
                    cacheType.Add(index, ColumnType.Boolean);
                else
                    cacheType.Add(index, ColumnType.Text);
            }
            return cacheType[index];
		}
        
        protected override bool IsType(int index, ColumnType type)
        {
            if (ColumnsDef.Any(c => c.Index == index && c.Type != type))
                return false;

            if (ColumnsDef.Any(c => c.Index == index && c.Type == type))
                return true;

            return (IsValue(index) && ValuesDefaultType == type);
        }
        
        public override Tolerance GetTolerance(int index)
		{
            if (GetColumnType(index) != ColumnType.Numeric && GetColumnType(index) != ColumnType.DateTime)
				return null;
			
			var col = ColumnsDef.FirstOrDefault(c => c.Index == index);
			if (col == null || !col.IsToleranceSpecified)
			{
				if (IsNumeric(index))
					return DefaultTolerance;
				else
					return DateTimeTolerance.None;
			}
				
			return ToleranceFactory.Instantiate(col);          
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

        protected SettingsResultSetComparisonByIndex(ColumnType valuesDefaultType, NumericTolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
        {}

        public SettingsResultSetComparisonByIndex(int columnsCount, KeysChoice keysDef, ValuesChoice valuesDef)
            : this(keysDef, valuesDef, ColumnType.Numeric, NumericAbsoluteTolerance.None, null)
		{
			ApplyTo(columnsCount);
		}

		public SettingsResultSetComparisonByIndex(KeysChoice keysDef, ValuesChoice valuesDef, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : this(keysDef, valuesDef, ColumnType.Numeric, NumericAbsoluteTolerance.None, columnsDef)
		{
		}

		public SettingsResultSetComparisonByIndex(KeysChoice keysDef, ValuesChoice valuesDef, NumericTolerance defaultTolerance)
			: this(keysDef, valuesDef, ColumnType.Numeric, defaultTolerance, null)
		{
		}

		public SettingsResultSetComparisonByIndex(KeysChoice keysDef, ValuesChoice valuesDef, ColumnType valuesDefaultType, NumericTolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
        {
            KeysDef = keysDef;
            ValuesDef = valuesDef;
            
        }
        
	}
}
