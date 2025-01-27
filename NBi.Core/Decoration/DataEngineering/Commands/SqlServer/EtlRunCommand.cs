using NBi.Core.Etl;
using NBi.Core.Decoration.DataEngineering;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Extensibility.Decoration.DataEngineering;

namespace NBi.Core.Decoration.DataEngineering;

public class EtlRunCommand : IDecorationCommand
{
    private readonly EtlRunCommandArgs args;

    public EtlRunCommand(EtlRunCommandArgs args) => this.args = args;

    public void Execute() => Execute(args.Etl);

    protected void Execute(IEtlArgs args)
    {
        var provider = new EtlRunnerProvider();
        var factory = provider.Instantiate(args.Version);
        var runner = factory.Instantiate(args);
        runner.Execute(); 
    }
}
