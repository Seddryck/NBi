using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

public class ScalarReaderArgs : IReaderArgs
{
    public IScalarResolver<string> Value { get; }

    public ScalarReaderArgs(IScalarResolver<string> value)
        => (Value) = (value);
}
