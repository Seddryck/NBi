using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening;

public interface IDataSerializationFlattenizer
{
    IEnumerable<object> Execute(TextReader textReader);
}
