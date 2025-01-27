using NBi.Extensibility;
using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Grouping.Commands;

class SequentialCommand : IGroupCommand
{
    private readonly IEnumerable<IDecorationCommand> commands;

    public SequentialCommand(IEnumerable<IDecorationCommand> commands, bool runOnce)
        => (this.commands, this.RunOnce) = (commands, runOnce);

    public bool RunOnce { get; set; }
    public bool HasRun { get; set; }
    public void Execute() => Execute(commands);

    internal void Execute(IEnumerable<IDecorationCommand> commands)
    {
        foreach (var command in commands)
            command.Execute();
    }
}
