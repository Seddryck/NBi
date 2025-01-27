using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class IoDeletePatternCommandArgs : IIoCommandArgs
{
    public IoDeletePatternCommandArgs(Guid guid, IScalarResolver<string> pattern, string basePath, IScalarResolver<string> path)
    {
        Guid = guid;
        Pattern = pattern;
        BasePath = basePath;
        Path = path;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> Pattern { get; }
    public string BasePath { get; }
    public IScalarResolver<string> Path { get; }
}
