using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Model.Decoration;

public class Setup : DecorationDefinition
{
    public Setup() { }

    public Setup(SetupStandalone standalone)
        => Commands = standalone.Commands;
}
