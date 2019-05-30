using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface ICopyExtensionCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> Extension { get; }
        IScalarResolver<string> DestinationPath { get; }
        IScalarResolver<string> SourcePath { get; }
    }
}
