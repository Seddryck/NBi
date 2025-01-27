using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class IoCopyPatternCommandArgs : IIoCommandArgs
{
    public IoCopyPatternCommandArgs(Guid guid, IScalarResolver<string> sourcePath, IScalarResolver<string> destinationPath, IScalarResolver<string> pattern, string basePath)
    {
        Guid = guid;
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
        Pattern = pattern;
        BasePath = basePath;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> SourcePath { get; }
    public IScalarResolver<string> DestinationPath { get; }
    public IScalarResolver<string> Pattern { get; }
    public string BasePath { get; }
}
