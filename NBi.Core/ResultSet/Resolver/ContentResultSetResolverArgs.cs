using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class ContentResultSetResolverArgs : RowsResultSetResolverArgs
{
    public IEnumerable<string> ColumnNames { get; }

    public ContentResultSetResolverArgs(IContent content)
        : base(content.Rows)
    {
        ColumnNames = content.Columns;
    }
}
