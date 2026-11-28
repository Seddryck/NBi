using NBi.Model.Decoration.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class CommandGroup : DecorationCommand
{
    public List<DecorationCommand> InternalCommands { get; set; }

    public List<DecorationCommand> Commands
    {
        get
        {
            return InternalCommands.Cast<DecorationCommand>().ToList();
        }
        set
        {
            InternalCommands = value.Cast<DecorationCommand>().ToList();
        }
    }

    public override Settings.SuiteSettings Settings
    {   get { return base.Settings; }
        set
        {
            base.Settings = value;
            foreach (var cmd in InternalCommands)
                cmd.Settings = value;
        }
    }

    public bool Parallel { get; set; }

    public bool RunOnce { get; set; }

    public bool HasRun { get; set; }

    public CommandGroup()
    {
        Parallel = true;
        RunOnce = false;
        HasRun = false;
        InternalCommands = new List<DecorationCommand>();
    }
}
