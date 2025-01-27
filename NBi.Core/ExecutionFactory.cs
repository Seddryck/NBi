using System;
using System.Linq;
using NBi.Core.Etl;
using NBi.Extensibility;
using NBi.Core.Decoration.DataEngineering;
using NBi.Extensibility.Decoration.DataEngineering;

namespace NBi.Core;

public class ExecutionFactory
{
    public IExecutable Instantiate(IExecutableArgs args)
    {
        return args switch
        {
            IEtlArgs etl => new EtlRunnerProvider().Instantiate(etl.Version).Instantiate(etl),
            _ => throw new ArgumentException(),
        };
    }
}
