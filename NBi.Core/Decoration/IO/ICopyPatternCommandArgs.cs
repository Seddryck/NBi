using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface ICopyPatternCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> Pattern { get; }
        IScalarResolver<string> DestinationPath { get; }
        IScalarResolver<string> SourcePath { get; }
    }
}
