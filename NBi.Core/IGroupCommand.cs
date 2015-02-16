using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public interface IGroupCommand : IDecorationCommand
    {
        List<IDecorationCommand> Commands { get; set; }
        bool Parallel { get; set; }
        bool RunOnce { get; set; }
        bool HasRun { get; set; }
    }
}
