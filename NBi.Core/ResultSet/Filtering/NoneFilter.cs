using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    class NoneFilter : IResultSetFilter
    {
        public IResultSet Apply(IResultSet rs)
            => rs ?? throw new ArgumentNullException();

        public IResultSet AntiApply(IResultSet rs)
        {
            var table = rs?.Table?.Clone() ?? throw new ArgumentNullException();
            var filteredRs = new ResultSet();
            filteredRs.Load(table);
            return filteredRs;
        }

        public string Describe() => "none";
    }
}
