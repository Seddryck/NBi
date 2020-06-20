using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface IDeletePatternCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> Pattern { get; }
        IScalarResolver<string> Path { get; }
    }
}
