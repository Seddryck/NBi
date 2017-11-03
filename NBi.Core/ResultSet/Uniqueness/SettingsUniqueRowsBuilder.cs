using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class SettingsUniqueRowsBuilder : SettingsResultSetBuilder
    {
        protected override void OnBuild()
        {
            BuildSettings(ColumnType.Text, ColumnType.Numeric, null);
        }
    }
}
