using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process;

public class WaitCommandArgs : IProcessCommandArgs
{
    public WaitCommandArgs(Guid guid, IScalarResolver<int> milliSeconds)
    {
        Guid = guid;
        MilliSeconds = milliSeconds;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<int> MilliSeconds { get; }
}
