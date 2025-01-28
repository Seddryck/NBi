using NBi.Core.ResultSet;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Grouping.ColumnBased;

public class ColumnGroupByArgs : IGroupByArgs
{

    public IEnumerable<IColumnDefinitionLight> Columns { get; set; }
    public Context Context { get; set; }

    public ColumnGroupByArgs(IEnumerable<IColumnDefinitionLight> columns, Context context)
    => (Columns, Context) = (columns, context);
}
