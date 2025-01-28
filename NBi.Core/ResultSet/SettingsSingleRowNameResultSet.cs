using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Scalar.Comparer;

namespace NBi.Core.ResultSet;

	public class SettingsSingleRowNameResultSet : SettingsNameResultSet, ISettingsSingleRowResultSet
{
    
		public SettingsSingleRowNameResultSet(ColumnType valuesDefaultType, Tolerance defaultTolerance, IReadOnlyCollection<IColumnDefinition> columnsDef)
        : base(valuesDefaultType, defaultTolerance, columnsDef)
		{ }

    public SettingsSingleRowNameResultSet()
        : this(ColumnType.Numeric, NumericAbsoluteTolerance.None, [])
    { }

    protected override bool IsKey(string name)
        => false;

    protected override bool IsValue(string name)
    {
        if (ColumnsDef.Any(c => (c.Identifier as ColumnNameIdentifier)?.Name == name && c.Role == ColumnRole.Ignore))
            return false;

        return true;
    }

}
