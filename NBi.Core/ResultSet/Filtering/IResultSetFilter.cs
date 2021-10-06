using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    public interface IResultSetFilter
    {
        IResultSet Apply(IResultSet rs);
        IResultSet AntiApply(IResultSet rs);

        string Describe();
    }
}
