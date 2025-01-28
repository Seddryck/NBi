using NBi.Core.DataSerialization.Flattening;
using NBi.Core.DataSerialization.Reader;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization;

public class DataSerializationProcessor
{
    public IDataSerializationFlattenizer Flattenizer { get; }
    public IDataSerializationReader Reader { get; }

    public DataSerializationProcessor(IDataSerializationReader reader, IDataSerializationFlattenizer flattenizer)
        => (Reader, Flattenizer) = (reader, flattenizer);

    public IEnumerable<object> Execute()
    {
        var textReader = Reader.Execute();
        var result = Flattenizer.Execute(textReader);
        Reader.Dispose();
        return result;
    }
}
