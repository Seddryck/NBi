using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class RowsResultSetResolverArgs : ResultSetResolverArgs
{
    public IList<IRow> Rows { get; }
    public RowsResultSetResolverArgs(IList<IRow> rows)
    {
        Rows = rows;
    }
}
