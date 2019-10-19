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
		{
		}

        public SettingsSingleRowOrdinalResultSet ()
            : this(ColumnType.Numeric, null, null)
        {
        }

        protected override bool IsKey(int index)
        {
            return false;
        }

        protected override bool IsValue(int index)
        {
            if (ColumnsDef.Any(c => (c.Identifier as ColumnOrdinalIdentifier)?.Ordinal == index && c.Role == ColumnRole.Ignore))
                return false;

            return true;
        }

    }
}
