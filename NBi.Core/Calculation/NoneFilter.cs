using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    class NoneFilter : IResultSetFilter
    {
        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            if (rs == null)
                throw new ArgumentNullException();
            return rs;
        }
    }
}
