using NBi.Core.DataSerialization.Flattening;
using NBi.Core.DataSerialization.Reader;
using NBi.Core.ResultSet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization;

public class DataSerializationProcessorFactory
{
    public DataSerializationProcessor Instantiate(DataSerializationResultSetResolverArgs args)
    {
        var reader = new DataSerializationReaderFactory().Instantiate(args.Reader);
        var flattenizer = new DataSerializationFlattenizerFactory().Instantiate(args.Flattenizer);
        return new DataSerializationProcessor(reader, flattenizer);
    }
}
