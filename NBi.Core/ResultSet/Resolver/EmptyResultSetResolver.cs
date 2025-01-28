using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class EmptyResultSetResolver : IResultSetResolver
{
    private EmptyResultSetResolverArgs Args { get; }

    public EmptyResultSetResolver(EmptyResultSetResolverArgs args)
        => Args = args;

    public virtual IResultSet Execute()
    {
        var dataTable = new DataTableResultSet();
        if (Args.Identifiers != null)
            foreach (var identifier in Args.Identifiers)
                dataTable.AddColumn(identifier.Name);

        if (Args.ColumnCount!=null && dataTable.ColumnCount< Args.ColumnCount.Execute())
        {
            var missingColumnCount = Args.ColumnCount.Execute() - dataTable.ColumnCount;
            for (int i = 0; i < missingColumnCount; i++)
                dataTable.AddColumn($"Column_{dataTable.ColumnCount}");
        }
        return dataTable;
    }
}
