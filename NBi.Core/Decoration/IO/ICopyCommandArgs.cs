using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface ICopyCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> SourceName { get; }
        IScalarResolver<string> SourcePath { get; }
        IScalarResolver<string> DestinationName { get; }
        IScalarResolver<string> DestinationPath { get; }
    }
}
