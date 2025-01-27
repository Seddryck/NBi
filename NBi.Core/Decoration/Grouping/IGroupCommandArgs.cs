using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Grouping;

public interface IGroupCommandArgs : IDecorationCommandArgs
{
    IEnumerable<IDecorationCommandArgs> Commands { get; }
    bool RunOnce { get; }
}
