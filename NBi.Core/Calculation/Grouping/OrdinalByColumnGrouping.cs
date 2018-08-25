using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Grouping
{
    class OrdinalByColumnGrouping : AbstractByColumnGrouping
    {
        protected new SettingsOrdinalResultSet Settings { get => base.Settings as SettingsOrdinalResultSet; }

        public OrdinalByColumnGrouping(SettingsOrdinalResultSet settings)
            : base(settings) { }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
            => new DataRowKeysComparerByOrdinal(Settings, x.Columns.Count);
    }
}
