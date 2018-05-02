using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Grouping
{
    public class ByColumnGroupingFactory
    {
        public IByColumnGrouping Instantiate(ISettingsResultSet settings)
        {

            if (settings is SettingsIndexResultSet)
                return new IndexByColumnGrouping(settings as SettingsIndexResultSet);

            else if (settings is SettingsNameResultSet)
                return new NameByColumnGrouping(settings as SettingsNameResultSet);

            throw new ArgumentOutOfRangeException(nameof(settings));
        }
    }
}
