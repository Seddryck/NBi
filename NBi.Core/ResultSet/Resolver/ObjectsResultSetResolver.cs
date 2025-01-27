using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

class ObjectsResultSetResolver : IResultSetResolver
{
    private readonly ObjectsResultSetResolverArgs args;

    public ObjectsResultSetResolver(ObjectsResultSetResolverArgs args)
    {
        this.args = args;
    }

    public virtual IResultSet Execute()
    {
        var helper = new ObjectsToRowsHelper();
        var rows = helper.Execute(args.Objects);

        var rs = new DataTableResultSet();
        rs.Load(rows);
        return rs;
    }

}
