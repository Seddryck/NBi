using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Reshaping
{
    public class UnstackArgs : IReshapingArgs
    {
        public IEnumerable<IColumnDefinitionLight> GroupBys { get; set; }
        public IColumnIdentifier Header { get; set; }

        public UnstackArgs(IColumnIdentifier header, IEnumerable<IColumnDefinitionLight> groupBys)
            => (Header, GroupBys) = (header, groupBys);
    }
}
