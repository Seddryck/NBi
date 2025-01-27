using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension;

abstract class AbstractExtendEngine : IExtensionEngine
{
    protected ServiceLocator ServiceLocator { get; }
    protected Context Context { get; }
    protected IColumnIdentifier NewColumn { get; }
    protected string Code { get; }

    public AbstractExtendEngine(ServiceLocator serviceLocator, Context context, IColumnIdentifier newColumn, string code)
        => (ServiceLocator, Context, NewColumn, Code) = (serviceLocator, context, newColumn, code);

    public IResultSet Execute(IResultSet rs)
    {
        var ordinal = GetNewColumnOrdinal(NewColumn, rs);
        return Execute(rs, ordinal);
    }

    protected abstract IResultSet Execute(IResultSet rs, int ordinal);
    
    private int GetNewColumnOrdinal(IColumnIdentifier newColumn, IResultSet rs)
    {
        switch (newColumn)
        {
            case ColumnOrdinalIdentifier o:
                if (o.Ordinal >= rs.ColumnCount)
                    return rs.AddColumn($"NO_NAME_{rs.ColumnCount}").Ordinal;
                else
                {
                    rs.AddColumn($"NO_NAME_{o.Ordinal}", o.Ordinal, typeof(object));
                    return o.Ordinal;
                };
            case ColumnNameIdentifier n:
                return (rs.Columns.FirstOrDefault(x => x.Name == n.Name)
                    ?? rs.AddColumn(n.Name)).Ordinal;
            default:
                throw new ArgumentException();
        }
    }
}
