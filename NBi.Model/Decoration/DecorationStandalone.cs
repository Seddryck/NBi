using System;
using System.Linq;

namespace NBi.Model.Decoration;

public class DecorationStandalone : DecorationDefinition
{
    public DecorationStandalone()
    { }

    public DecorationStandalone(DecorationDefinition full)
    {
        Commands = full.Commands;
    }
}

public class SetupStandalone : DecorationStandalone { }

public class CleanupStandalone : DecorationStandalone { }
