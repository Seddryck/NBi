using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO
{
    public interface IDeleteCommandArgs : IIoCommandArgs
    {
        IScalarResolver<string> Name { get; }
        IScalarResolver<string> Path { get; }
    }
}
