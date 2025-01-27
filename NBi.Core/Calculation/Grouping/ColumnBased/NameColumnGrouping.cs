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

class NameColumnGrouping : ColumnGrouping
{
    protected new SettingsNameResultSet Settings 
        => (base.Settings as SettingsNameResultSet)!; 

    public NameColumnGrouping(SettingsNameResultSet settings, Context context)
        : base(settings, context) { }

    protected override DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x)
        => new DataRowKeysComparerByName(Settings);
}
