using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class IoCopyCommandArgs : IIoCommandArgs
{
    public IoCopyCommandArgs(Guid guid, IScalarResolver<string> sourceName, IScalarResolver<string> sourcePath, IScalarResolver<string> destinationName, IScalarResolver<string> destinationPath, string basePath)
    {
        Guid = guid;
        SourceName = sourceName;
        SourcePath = sourcePath;
        DestinationName = destinationName;
        DestinationPath = destinationPath;
        BasePath = basePath;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> SourceName { get; }
    public IScalarResolver<string> SourcePath { get; }
    public IScalarResolver<string> DestinationName { get; }
    public IScalarResolver<string> DestinationPath { get; }
    public string BasePath { get; }
}
