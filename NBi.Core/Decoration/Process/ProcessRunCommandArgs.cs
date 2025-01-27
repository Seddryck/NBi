using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Extensibility.Resolving;

namespace NBi.Core.Decoration.Process;

public class ProcessRunCommandArgs : IProcessCommandArgs
{
    public ProcessRunCommandArgs(Guid guid, IScalarResolver<string> name, IScalarResolver<string> path, string basePath, IScalarResolver<string> argument, IScalarResolver<int> timeOut)
    {
        Guid = guid;
        Name = name;
        Path = path;
        BasePath = basePath;
        Argument = argument;
        TimeOut = timeOut;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> Name { get; }
    public IScalarResolver<string> Path { get; }
    public string BasePath { get; }
    public IScalarResolver<string> Argument { get; }
    public IScalarResolver<int> TimeOut { get; set; }
}
