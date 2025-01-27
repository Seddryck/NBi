using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Grouping;

public class GroupParallelCommandArgs : IGroupCommandArgs
{
    public GroupParallelCommandArgs(Guid guid, bool runOnce, IEnumerable<IDecorationCommandArgs> commands)
    {
        Guid = guid;
        RunOnce = runOnce;
        Commands = commands;
    }
    
    public Guid Guid { get; set; }
    public bool RunOnce { get; }
    public IEnumerable<IDecorationCommandArgs> Commands { get; }
}
