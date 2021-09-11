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
            var ordinal = GetNewColumnOrdinal(NewColumn, rs);
            return Execute(rs, ordinal);
        }

        protected abstract IResultSet Execute(IResultSet rs, int ordinal);
        
        private int GetNewColumnOrdinal(IColumnIdentifier newColumn, IResultSet rs)
        {
            switch (newColumn)
            {
                case ColumnOrdinalIdentifier o:
                    if (o.Ordinal >= rs.Columns.Count)
                        return rs.Columns.Add($"NO_NAME_{rs.Columns.Count}", typeof(object)).Ordinal;
                    else
                    {
                        rs.Columns.Add($"NO_NAME_{o.Ordinal}", typeof(object)).SetOrdinal(o.Ordinal);
                        return o.Ordinal;
                    };
                case ColumnNameIdentifier n:
                    return (rs.Columns.Cast<DataColumn>().FirstOrDefault(x => x.ColumnName == n.Name)
                        ?? rs.Columns.Add(n.Name, typeof(object))).Ordinal;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
