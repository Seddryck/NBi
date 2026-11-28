using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Model.Decoration;

public class Cleanup : DecorationDefinition
{
    public Cleanup() { }

    public Cleanup(CleanupStandalone standalone)
        => Commands = standalone.Commands;
}
