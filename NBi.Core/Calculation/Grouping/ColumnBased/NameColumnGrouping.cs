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
    class NameColumnGrouping : ColumnGrouping
    {
        protected new SettingsNameResultSet Settings { get => base.Settings as SettingsNameResultSet; }

        public NameColumnGrouping(SettingsNameResultSet settings, Context context)
            : base(settings, context) { }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
            => new DataRowKeysComparerByName(Settings);
    }
}
