using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

public class UrlReaderArgs : IReaderArgs
{
    public IScalarResolver<string> Url { get; }

    public UrlReaderArgs(IScalarResolver<string> url)
        => (Url) = (url);
}
