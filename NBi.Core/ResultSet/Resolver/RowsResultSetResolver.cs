using NBi.Core.Query;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class RowsResultSetResolver : IResultSetResolver
{
    private readonly RowsResultSetResolverArgs args;

    public RowsResultSetResolver(RowsResultSetResolverArgs args)
    {
        this.args = args;
    }

    public virtual IResultSet Execute()
    {
        var rs = new DataTableResultSet();
        rs.Load(args.Rows);
        return rs;
        }
}
