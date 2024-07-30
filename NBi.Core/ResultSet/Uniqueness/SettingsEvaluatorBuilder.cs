using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class SettingsEvaluatorBuilder : SettingsResultSetBuilder
    {
        protected override void OnBuild()
        {
            BuildSettings(ColumnType.Text, ColumnType.Numeric, NumericAbsoluteTolerance.None);
        }

        protected override void OnCheck()
        {
            PerformInconsistencyChecks();
            PerformSetsAndColumnsCheck(
                SettingsOrdinalResultSet.KeysChoice.All
                , SettingsOrdinalResultSet.ValuesChoice.None);
            PerformDuplicationChecks();
        }
    }
}
