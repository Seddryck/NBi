using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
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
            var ordinal = GetNewColumnOrdinal(NewColumn, rs.Table);
            return Execute(rs, ordinal);
        }

        protected abstract IResultSet Execute(IResultSet rs, int ordinal);
        
        private int GetNewColumnOrdinal(IColumnIdentifier newColumn, DataTable dt)
        {
            switch (newColumn)
            {
                case ColumnOrdinalIdentifier o:
                    if (o.Ordinal >= dt.Columns.Count)
                        return dt.Columns.Add($"NO_NAME_{dt.Columns.Count}", typeof(object)).Ordinal;
                    else
                    {
                        dt.Columns.Add($"NO_NAME_{o.Ordinal}", typeof(object)).SetOrdinal(o.Ordinal);
                        return o.Ordinal;
                    };
                case ColumnNameIdentifier n:
                    return (dt.Columns.Cast<DataColumn>().FirstOrDefault(x => x.ColumnName == n.Name)
                        ?? dt.Columns.Add(n.Name, typeof(object))).Ordinal;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
