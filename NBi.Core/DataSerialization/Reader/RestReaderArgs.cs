using NBi.Core.Api.Rest;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

public class RestReaderArgs : IReaderArgs
{
    public RestEngine Rest { get; }

    public RestReaderArgs(RestEngine rest)
        => (Rest) = (rest);
}
