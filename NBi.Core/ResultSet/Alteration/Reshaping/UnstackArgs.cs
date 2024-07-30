using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Reshaping
{
    public class UnstackArgs : IReshapingArgs
    {
        public IColumnIdentifier Header { get; set; }
        public IEnumerable<IColumnDefinitionLight> GroupBys { get; set; }
        public IEnumerable<ColumnNameIdentifier> EnforcedColumns { get; set; }

        public UnstackArgs(IColumnIdentifier header, IEnumerable<IColumnDefinitionLight> groupBys)
            : this(header, groupBys, []) { }

        public UnstackArgs(IColumnIdentifier header, IEnumerable<IColumnDefinitionLight> groupBys, IEnumerable<ColumnNameIdentifier> enforcedColumns)
            => (Header, GroupBys, EnforcedColumns) = (header, groupBys, enforcedColumns);
    }
}
