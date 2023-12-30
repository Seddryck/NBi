using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Scalar.Comparer;

namespace NBi.Core.ResultSet
{
	public abstract class SettingsResultSet<T> : ISettingsResultSet where T : notnull
    {
        protected IDictionary<T, ColumnRole> cacheRole = new Dictionary<T, ColumnRole>();
        protected IDictionary<T, ColumnType> cacheType = new Dictionary<T, ColumnType>();

        protected ColumnType ValuesDefaultType { get; private set; }
		protected IReadOnlyCollection<IColumnDefinition> ColumnsDef { get; private set; }

        private Tolerance defaultTolerance = NumericAbsoluteTolerance.None;
        protected Tolerance DefaultTolerance
        {
            get { return defaultTolerance; }
            private set { defaultTolerance = value; }
        }

        protected abstract bool IsKey(T index);
        protected abstract bool IsValue(T index);

        public abstract Tolerance? GetTolerance(T index);
        public abstract bool IsRounding(T index);
        public abstract Rounding? GetRounding(T index);
        
        public abstract ColumnRole GetColumnRole(T index);
        public abstract ColumnType GetColumnType(T index);
        protected virtual bool IsNumeric(T index)
        {
            return IsType(index, ColumnType.Numeric);
        }

        protected virtual bool IsDateTime(T index)
        {
            return IsType(index, ColumnType.DateTime);
        }

        protected virtual bool IsBoolean(T index)
        {
            return IsType(index, ColumnType.Boolean);
        }

        protected virtual bool IsText(T index)
        {
            return IsType(index, ColumnType.Text);
        }

        protected abstract bool IsType(T index, ColumnType type);

        public SettingsResultSet(ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
        {
            ValuesDefaultType = valuesDefaultType;
            DefaultTolerance = defaultTolerance;
            if (columnsDef != null)
                ColumnsDef = columnsDef;
            else
                ColumnsDef = new List<IColumnDefinition>(0);
        }
        
	}
}
