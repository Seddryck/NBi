using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public interface IIoCommandArgs : IDecorationCommandArgs
{
    string BasePath { get; }
}
