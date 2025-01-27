using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core;

public interface IGroupCommand : IDecorationCommand
{
    bool RunOnce { get; set; }
    bool HasRun { get; set; }
}
