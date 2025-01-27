using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Extensibility.Resolving;

namespace NBi.Core.Decoration.Process;

public class ProcessKillCommandArgs : IProcessCommandArgs
{
    public ProcessKillCommandArgs(Guid guid, IScalarResolver<string> processName)
    {
        Guid = guid;
        ProcessName = processName;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> ProcessName { get; }
}
