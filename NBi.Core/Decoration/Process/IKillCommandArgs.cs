using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Scalar.Resolver;

namespace NBi.Core.Decoration.Process
{
    public interface IKillCommandArgs : IProcessCommandArgs
    {
        IScalarResolver<string> ProcessName { get; }
    }
}
