using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process
{
    public interface IStartCommandArgs : IProcessCommandArgs
    {
        IScalarResolver<string> ServiceName { get; }
        IScalarResolver<int> TimeOut { get; }
    }
}
