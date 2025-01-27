using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet.Alteration.Merging;

public class UnionArgs : IMergingArgs
{
    public IResultSetResolver ResultSetResolver { get; }
    public ColumnIdentity Identity { get; }

    public UnionArgs(IResultSetResolver resultSetResolver, ColumnIdentity identity)
        => (ResultSetResolver, Identity) = (resultSetResolver, identity);
}

public enum ColumnIdentity
{
    [XmlEnum("ordinal")]
    Ordinal = 0,
    [XmlEnum("name")]
    Name = 1,
}
