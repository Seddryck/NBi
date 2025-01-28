using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;

namespace NBi.Core.Calculation.Grouping.ColumnBased;

class OrdinalColumnGrouping : ColumnGrouping
{
    protected new SettingsOrdinalResultSet Settings
        => (base.Settings as SettingsOrdinalResultSet)!;

    public OrdinalColumnGrouping(SettingsOrdinalResultSet settings, Context context)
        : base(settings, context) { }

    protected override DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x)
        => new DataRowKeysComparerByOrdinal(Settings, x.ColumnCount);
}
