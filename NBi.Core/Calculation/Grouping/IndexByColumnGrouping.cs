using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Grouping
{
    class IndexByColumnGrouping : AbstractByColumnGrouping
    {
        protected new SettingsIndexResultSet Settings { get => base.Settings as SettingsIndexResultSet; }

        public IndexByColumnGrouping(SettingsIndexResultSet settings)
            : base(settings) { }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
            => new DataRowKeysComparerByIndex(Settings, x.Columns.Count);
    }
}
