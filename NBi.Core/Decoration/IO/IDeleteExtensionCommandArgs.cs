using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface IDeleteExtensionCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> Extension { get; }
        IScalarResolver<string> Path { get; }
    }
}
