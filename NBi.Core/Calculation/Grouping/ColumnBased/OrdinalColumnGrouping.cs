using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using NBi.Core.Variable;

namespace NBi.Core.Calculation.Grouping.ColumnBased
{
    class OrdinalColumnGrouping : ColumnGrouping
    {
        protected new SettingsOrdinalResultSet Settings { get => base.Settings as SettingsOrdinalResultSet; }

        public OrdinalColumnGrouping(SettingsOrdinalResultSet settings, Context context)
            : base(settings, context) { }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
            => new DataRowKeysComparerByOrdinal(Settings, x.Columns.Count);
    }
}
