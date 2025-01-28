using NBi.Core.Query;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class EmptyResultSetResolverArgs : ResultSetResolverArgs
{
    public IScalarResolver<int> ColumnCount { get; } = new LiteralScalarResolver<int>(0);
    public IEnumerable<ColumnNameIdentifier> Identifiers { get; } = [];
    public EmptyResultSetResolverArgs(IEnumerable<ColumnNameIdentifier> columns, IScalarResolver<int> columnCount)
        => (Identifiers, ColumnCount) = (columns, columnCount);

    public EmptyResultSetResolverArgs(IEnumerable<ColumnNameIdentifier> columns)
        => (Identifiers, ColumnCount) = (columns, new LiteralScalarResolver<int>(0));

    public EmptyResultSetResolverArgs(IScalarResolver<int> columnCount)
        => (ColumnCount) = (columnCount);
}
