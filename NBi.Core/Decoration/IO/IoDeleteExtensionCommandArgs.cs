using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class IoDeleteExtensionCommandArgs : IIoCommandArgs
{
    public IoDeleteExtensionCommandArgs(Guid guid, IScalarResolver<string> extension, string basePath, IScalarResolver<string> path)
    {
        Guid = guid;
        Extension = extension;
        BasePath = basePath;
        Path = path;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> Extension { get; }
    public string BasePath { get; }
    public IScalarResolver<string> Path { get; }
}
