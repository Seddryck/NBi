using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.DataSerialization;
using NBi.Core.DataSerialization.Reader;
using NBi.Core.DataSerialization.Flattening;

namespace NBi.Core.ResultSet.Resolver;

public class DataSerializationResultSetResolverArgs : ResultSetResolverArgs
{
    public IReaderArgs Reader { get; }
    public IFlattenizerArgs Flattenizer { get; }

    public DataSerializationResultSetResolverArgs(IReaderArgs reader, IFlattenizerArgs flattenizer)
        => (Reader, Flattenizer) = (reader, flattenizer);
}
