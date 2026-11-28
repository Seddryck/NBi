using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Decoration.Command;

public class CustomCommand : DecorationCommand
{
    public string AssemblyPath { get; set; }

    public string TypeName { get; set; }

    public List<CustomCommandParameter> Parameters { get; set; } = new List<CustomCommandParameter>();
}
