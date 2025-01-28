using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class IoCopyExtensionCommandArgs : IIoCommandArgs
{
    public IoCopyExtensionCommandArgs(Guid guid, IScalarResolver<string> sourcePath, IScalarResolver<string> destinationPath, IScalarResolver<string> extension, string basePath)
    {
        Guid = guid;
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
        Extension = extension;
        BasePath = basePath;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> SourcePath { get; }
    public IScalarResolver<string> DestinationPath { get; }
    public IScalarResolver<string> Extension { get; }
    public string BasePath { get; }
}
