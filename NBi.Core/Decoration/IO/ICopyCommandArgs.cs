using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface ICopyCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> DestinationName { get; }
        IScalarResolver<string> DestinationPath { get; }
    }
}
