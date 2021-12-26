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
            => rs?.Clone() ?? throw new ArgumentNullException();

        public string Describe() => "none";
    }
}
