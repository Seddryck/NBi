using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    public interface IResultSetFilter
    {
        ResultSet Apply(ResultSet rs);
        ResultSet AntiApply(ResultSet rs);

        string Describe();
    }
}
