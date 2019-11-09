using NBi.Core.Query;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class EmptyResultSetResolverArgs : ResultSetResolverArgs
    {
        public IScalarResolver<int> ColumnCount { get; } = null;
        public IEnumerable<ColumnNameIdentifier> Identifiers { get; } = null;
        public EmptyResultSetResolverArgs(IEnumerable<ColumnNameIdentifier> columns, IScalarResolver<int> columnCount)
            => (Identifiers, ColumnCount) = (columns, columnCount);

        public EmptyResultSetResolverArgs(IEnumerable<ColumnNameIdentifier> columns)
            => (Identifiers, ColumnCount) = (columns, null);

        public EmptyResultSetResolverArgs(IScalarResolver<int> columnCount)
            => (ColumnCount) = (columnCount);
    }
}
