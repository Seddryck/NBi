using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class IoDeleteCommandArgs : IIoCommandArgs
{
    public IoDeleteCommandArgs(Guid guid, IScalarResolver<string> name, string basePath, IScalarResolver<string> path)
    {
        Guid = guid;
        Name = name;
        BasePath = basePath;
        Path = path;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> Name { get; }
    public string BasePath { get; }
    public IScalarResolver<string> Path { get; }
}
