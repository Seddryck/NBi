using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    class NoneFilter : IResultSetFilter
    {
        public ResultSet Apply(ResultSet rs)
        {
            if (rs == null)
                throw new ArgumentNullException();
            return rs;
        }

        public ResultSet AntiApply(ResultSet rs)
        {
            if (rs == null)
                throw new ArgumentNullException();

            var filteredRs = new ResultSet();
            var table = rs.Table.Clone();
            filteredRs.Load(table);
            return filteredRs;
        }

        public string Describe() => "none";
    }
}
