using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.ResultSet.Resolver;

namespace NBi.Core.ResultSet;

public class Content : IContent
{
    public IList<IRow> Rows { get; set; }
    public IList<string> Columns { get; set; }

    public Content(IList<IRow> rows, IList<string> columns)
    {
        Rows = rows;
        Columns = columns;
    }
}
