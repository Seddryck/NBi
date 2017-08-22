using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet
{
	public class SettingsSingleRowComparison: SettingsResultSetComparisonByIndex
	{
        
		public SettingsSingleRowComparison(ColumnType valuesDefaultType, NumericTolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
            : base(valuesDefaultType, defaultTolerance, columnsDef)
		{
		}

        public SettingsSingleRowComparison()
            : this(ColumnType.Numeric, null, null)
        {
        }

        protected override bool IsKey(int index)
        {
            return false;
        }

        protected override bool IsValue(int index)
        {
            if (ColumnsDef.Any(c => c.Index == index && c.Role == ColumnRole.Ignore))
                return false;

            return true;
        }

    }
}
