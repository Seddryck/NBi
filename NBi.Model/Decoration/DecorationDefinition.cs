using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Decoration.Command;

namespace NBi.Model.Decoration;

public abstract class DecorationDefinition
{
    public List<DecorationCommand> Commands { get; set; }


    public DecorationDefinition()
    {
        Commands = new List<DecorationCommand>();
    }
}
