using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tasks = System.Threading.Tasks;

namespace NBi.Core.Decoration.Grouping.Commands;

class ParallelCommand : IGroupCommand
{
    private readonly IEnumerable<IDecorationCommand> commands;

    public ParallelCommand(IEnumerable<IDecorationCommand> commands, bool runOnce) 
        => (this.commands, this.RunOnce) = (commands, runOnce);

    public bool RunOnce { get; set; }
    public bool HasRun { get; set; }

    public void Execute() => Execute(commands);

    internal void Execute(IEnumerable<IDecorationCommand> commands)
    {
        Tasks.Parallel.ForEach
            (
                commands,
                x => x.Execute()
            );
    }
}
