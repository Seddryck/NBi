using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS = NBi.Core.ResultSet;

namespace NBi.Core.Calculation
{
    public interface IResultSetFilter
    {
        RS.ResultSet Apply(RS.ResultSet rs);
        RS.ResultSet AntiApply(RS.ResultSet rs);

        string Describe();
    }
}
