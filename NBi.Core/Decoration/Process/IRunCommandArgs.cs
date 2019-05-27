using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Scalar.Resolver;

namespace NBi.Core.Decoration.Process
{
    public interface IRunCommandArgs : IProcessCommandArgs
    {
        IScalarResolver<string> Name { get; }
        IScalarResolver<string> Path { get; }
        IScalarResolver<string> Argument { get; }
        IScalarResolver<int> TimeOut { get; set; }
    }
}
