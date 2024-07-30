using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Scalar.Comparer;

namespace NBi.Core.ResultSet
{
	public class SettingsSingleRowOrdinalResultSet : SettingsOrdinalResultSet, ISettingsSingleRowResultSet
    {
        
		public SettingsSingleRowOrdinalResultSet (ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
		{ }

        public SettingsSingleRowOrdinalResultSet ()
            : this(ColumnType.Numeric, NumericAbsoluteTolerance.None, [])
        { }

        protected override bool IsKey(int index)
            => false;

        protected override bool IsValue(int index)
            => !ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Role == ColumnRole.Ignore);
    }
}
